<?
/**
 * Abramov Anton 2017
 * Routes of application
 */

/**
 * Main screen of application
 */
$app->get('/', function ($request, $response, $arguments) {

	return $this->renderer->render($response, '/index.html', $arguments)
		->withHeader('Content-Type', CONTENT_TYPE_HTML);
});

/*
 * Products
 */

/**
 * List of products by GET
 */
$app->get('/products', function ($request, $response) {
	$items = [];
	$res = DB::get("SELECT id, name FROM `product` WHERE active = 1;");
	if(!$res->empty)
		foreach ($res->data as $item)
			if(isset($item['id']) && !empty($item['id']) && isset($item['name']) && !empty($item['name']))
				$items[$item['id']] = $item['name'];

	if(!empty($items)){
		$html = '<div class="items">';
		foreach ($items as $key => $value)
			$html .= '<a class="item" href="/products/editor?id=' . $key . '">' . $value . '<span data-id="' . $key . '" class="order_btn">В заказ</span></a>';
		$html .= '</div>';
	}

	return $response->withJson([
		'success' => isset($res) && $res->success ? $res->success : false,
		'items'	=> isset($res) && !$res->empty ? $items : false,
		'error' => isset($error) ? $error : false,
		'template' => isset($html) ? $html : false
	], 200);
});

/**
 * Render page with form for search product by GET
 */
$app->get('/products/list', function ($request, $response, $arguments) {
	return $this->renderer->render($response, '/products.list.html', $arguments)
		->withHeader('Content-Type', CONTENT_TYPE_HTML);
});

/**
 * Render page with form for create product by POST
 */
$app->get('/products/create', function ($request, $response, $arguments) {
	return $this->renderer->render($response, '/products.create.html', $arguments)
		->withHeader('Content-Type', CONTENT_TYPE_HTML);
});

/**
 * Render page with form for search product by GET
 */
$app->get('/products/edit', function ($request, $response, $arguments) {
	return $this->renderer->render($response, '/products.edit.html', $arguments)
		->withHeader('Content-Type', CONTENT_TYPE_HTML);
});

/**
 * Check exist product by id in GET
 */
$app->get('/products/exist', function ($request, $response, $arguments) {
	$id = $request->getParam('id');
	if($id != null && !empty($id)) {
		$res = DB::get("SELECT id FROM `product` WHERE id = " . $id . " AND active = 1;");
		if($res->empty)
			$error = 'Элемент не найден';
	}

	return $response->withJson([
		'success' => isset($res) && $res->success && !$res->empty ? $res->success : false,
		'link'	=> isset($res) && !$res->empty ? '/products/editor?id=' . $id : false,
		'error' => isset($error) ? $error : false,
		'res' => $res
	], 200);
});

/**
 * Render page with form for edit product by PUT
 */
$app->get('/products/editor', function ($request, $response, $arguments) {
	$id = $request->getParam('id');
	if($id != null && !empty($id)) {
		$res = DB::get("SELECT id FROM `product` WHERE id = " . $id . " AND active = 1;");
		if($res->empty)
			return $response->withJson([
				'success' => false,
				'error' => 'Элемент не найден!'
			], 404);
	}

	return $this->renderer->render($response, '/products.editor.html', ['id' => $id])
		->withHeader('Content-Type', CONTENT_TYPE_HTML);
});

/**
 * Load data about product by id in GET
 */
$app->get('/products/editor/load', function ($request, $response, $arguments) {
	$id = $request->getParam('id');
	if($id != null && !empty($id))
		$res = DB::get("SELECT * FROM `product` WHERE id = " . $id . " AND active = 1 LIMIT 1;");

	if(isset($res) && $res->success && !$res->empty){
		$item = (array)$res->data[0];

		if(isset($item['stores']) && !empty($item['stores'])){
			$stores = json_decode($item['stores']);
			if($stores && !empty($stores)){
				$s = [];
				$c = [];
				$stores = (array)$stores;
				foreach ($stores as $store){
					foreach($store as $id => $count){
						$s[] = $id;
						$c[] = $count;
					}
				}

				$count = count($s);
				if(count($c) == $count && $count > 0){
					$item['stores'] = $s;
					$item['counts'] = $c;
				}
			}
		}
	}

	return $response->withJson([
		'success' => isset($res) && $res->success ? true : false,
		'error' => isset($error) ? $error : false,
		'item' => isset($res) && !empty($item) && is_array($item) ? $item : false
	], 200);
});

/**
 * Search of products by GET
 */
$app->get('/products/search', function ($request, $response) {
	$query = $request->getParam('query');
	if($query != null && !empty($query)){
		if(strlen($query) > 3){

			$items = [];
			$res = DB::get("SELECT id, name FROM `product` WHERE MATCH(name, article) AGAINST ('" . Utils::clearSQL($query) . "') AND active = 1;");
			if(!$res->empty)
				foreach ($res->data as $item)
					if(isset($item['id']) && !empty($item['id']) && isset($item['name']) && !empty($item['name']))
						$items[$item['id']] = $item['name'];

			if(!empty($items)){
				$html = '<div class="items">';
				foreach ($items as $key => $value)
					$html .= '<a class="item" href="/products/editor?id=' . $key . '">' . $value . '<span data-id="' . $key . '" class="order_btn">В заказ</span></a>';
				$html .= '</div>';
			}
		}
		else $error = 'Строка поиска должна быть от 4-ех символов';

	}else $error = 'Не азадана строка поиска.';

	return $response->withJson([
		'success' => isset($res) && $res->success ? $res->success : false,
		'items'	=> isset($res) && !$res->empty ? $items : false,
		'error' => isset($error) ? $error : false,
		'template' => isset($html) ? $html : false
	], 200);
});

/**
 * Create product by POST
 */
$app->post('/products', function ($request, $response) {
	$post = $request->getParsedBody(); //parse POST data

	// get [{store_id: count}] data
	$stores = [];
	if(isset($post['stores']) && isset($post['counts'])){
		if(is_array($post['stores']) && is_array($post['counts'])){
			if(!empty($post['stores']) && !empty($post['counts'])){
				if(count($post['stores']) == count($post['counts'])){
					$length = count($post['stores']);
					$_stores = [];
					for($i = 0; $i < $length; $i++)
						$_stores[] = [$post['stores'][$i] => $post['counts'][$i]];

					array_walk_recursive($_stores, function($item, $key) use (&$stores){
						$stores[$key] = isset($stores[$key]) ?  $item + $stores[$key] : $item;
					});
					$stores = [$stores];
				}
			}
		}
	}

	if((!isset($post['name']) || empty($post['name'])) || (!isset($post['weight']) || empty($post['weight'])) ||
		(!isset($post['length']) || empty($post['length'])) || (!isset($post['width']) || empty($post['width'])) ||
		(!isset($post['volume']) || empty($post['volume'])) || (!isset($post['cost']) || empty($post['cost'])) ||
		(!isset($stores) || empty($stores)) || (!isset($post['producer']) || empty($post['producer'])))
		$error = 'Не все обязательные параметры были переданы.';

	if(!isset($error))
		$success = DB::exec('product.create', [
			isset($post['article']) && !empty($post['article']) ? $post['article'] : uniqid(),
			$post['name'],
			$post['weight'],
			$post['length'],
			$post['width'],
			$post['volume'],
			$post['cost'],
			isset($post['days_wait']) ? $post['days_wait'] : null,
			json_encode($stores),
			$post['producer'],
		])->success;

	return $response->withJson([
		'success' => isset($success) ? $success : false,
		'error' => isset($error) ? $error : false,
		'message' => isset($success) && !isset($error) ? 'Товар зарегистрирован в базе данных.' : false
	], 200);
});

/**
 * Update info about product by id in PUT
 */
$app->put('/products', function ($request, $response)  {
	$put = $request->getParsedBody(); //parse PUT data

	// get [{store_id: count}] data
	$stores = [];
	if(isset($put['stores']) && isset($put['counts'])){
		if(is_array($put['stores']) && is_array($put['counts'])){
			if(!empty($put['stores']) && !empty($put['counts'])){
				if(count($put['stores']) == count($put['counts'])){
					$length = count($put['stores']);
					$_stores = [];
					for($i = 0; $i < $length; $i++)
						$_stores[] = [$put['stores'][$i] => $put['counts'][$i]];

					array_walk_recursive($_stores, function($item, $key) use (&$stores){
						$stores[$key] = isset($stores[$key]) ?  $item + $stores[$key] : $item;
					});
					$stores = [$stores];
				}
			}
		}
	}

	if((!isset($put['name']) || empty($put['name'])) || (!isset($put['weight']) || empty($put['weight'])) ||
		(!isset($put['length']) || empty($put['length'])) || (!isset($put['width']) || empty($put['width'])) ||
		(!isset($put['volume']) || empty($put['volume'])) || (!isset($put['cost']) || empty($put['cost'])) ||
		(!isset($stores) || empty($stores)) || (!isset($put['producer']) || empty($put['producer'])) ||
		(!isset($put['id']) || empty($put['id'])))
		$error = 'Не все обязательные параметры были переданы.';

	if(!isset($error))
		$success = DB::exec('product.update', [
			isset($put['article']) && !empty($put['article']) ? $put['article'] : null,
			$put['name'],
			$put['weight'],
			$put['length'],
			$put['width'],
			$put['volume'],
			$put['cost'],
			isset($put['days_wait']) ? $put['days_wait'] : null,
			json_encode($stores),
			$put['producer'],
			$put['id']
		])->success;

	return $response->withJson([
		'success' => isset($success) ? $success : false,
		'error' => isset($error) ? $error : false,
		'params' => $put,
	], 200);
});

/**
 * Delete product by id in DELETE
 */
$app->delete('/products', function ($request, $response)  {
	$delete = $request->getParsedBody(); //parse DELETE data

	if(isset($delete['id']) && !empty($delete['id']))
		$check = DB::get("SELECT * FROM `product` WHERE id = " . $delete['id'] . " AND active = 1;");

	if(isset($check)){
		$success = DB::exec('product.delete', [
			$delete['id']
		])->success;
	}else
		$error = 'Элемент не найден!';

	return $response->withJson([
		'params' => $delete,
		'success' => isset($success) ? $success : false,
		'error' => isset($error) ? $error : false
	], 200);
});

/*
 * Producers
 */

/**
 * List of producers by GET
 */
$app->get('/producers', function ($request, $response) {
	$items = [];
	$res = DB::get("SELECT * FROM `producer` WHERE active = 1;");
	if(!$res->empty){
		foreach ($res->data as $item){
			if(!isset($item['id']) || empty($item['id']) || !isset($item['name']) || empty($item['name']))
				continue;
			$items[$item['id']] = $item['name'];

			if(isset($item['guarantee']) && !empty($item['guarantee']))
				$items[$item['id']] .= ' (' . $item['guarantee'] . ')';
		}
	}

	if(!empty($items)){
		$html = '<div class="items">';
		foreach ($items as $key => $value)
			$html .= '<a class="item" href="/producers/editor?id=' . $key . '">' . $value . '</a>';
		$html .= '</div>';
	}

	return $response->withJson([
		'success' => isset($res) && $res->success ? $res->success : false,
		'items'	=> $res->empty == false ? $items : false,
		'error' => isset($error) ? $error : false,
		'template' => isset($html) ? $html : false
	], 200);
});

/**
 * Render page with form for create producer by POST
 */
$app->get('/producers/create', function ($request, $response, $arguments) {
	return $this->renderer->render($response, '/producers.create.html', $arguments)
		->withHeader('Content-Type', CONTENT_TYPE_HTML);
});

/**
 * Create producer by POST
 */
$app->post('/producers', function ($request, $response) {
	$post = $request->getParsedBody(); //parse POST data

	if(!isset($post['name']) || empty($post['name']))
		$error = 'Не все обязательные параметры были переданы.';

	if(!isset($error)){
		$success = DB::exec('producer.create', [
			$post['name'],
			(isset($post['guarantee']) && !empty($post['guarantee']) ? $post['guarantee'] : null)
		])->success;
	}

	return $response->withJson([
		'success' => isset($success) ? $success : false,
		'error' => isset($error) ? $error : false,
		'message' => isset($success) && !isset($error) ? 'Производитель зарегистрирован в базе данных.' : false
	], 200);
});

/**
 * Render page with form for search producers by GET
 */
$app->get('/producers/list', function ($request, $response, $arguments) {
	return $this->renderer->render($response, '/producers.list.html', $arguments)
		->withHeader('Content-Type', CONTENT_TYPE_HTML);
});

/**
 * Search of producers by GET
 */
$app->get('/producers/search', function ($request, $response) {
	$query = $request->getParam('query');
	if($query != null && !empty($query)){
		if(strlen($query) > 3){

			$items = [];
			$res = DB::get("SELECT id, name FROM `producer` WHERE MATCH(name) AGAINST ('" . Utils::clearSQL($query) . "') AND active = 1;");
			if(!$res->empty)
				foreach ($res->data as $item)
					if(isset($item['id']) && !empty($item['id']) && isset($item['name']) && !empty($item['name']))
						$items[$item['id']] = $item['name'];

			if(!empty($items)){
				$html = '<div class="items">';
				foreach ($items as $key => $value)
					$html .= '<a class="item" href="/producers/editor?id=' . $key . '">' . $value . '</a>';
				$html .= '</div>';
			}
		}
		else $error = 'Строка поиска должна быть от 4-ех символов';

	}else $error = 'Не азадана строка поиска.';

	return $response->withJson([
		'success' => isset($res) && $res->success ? $res->success : false,
		'items'	=> isset($res) && !$res->empty ? $items : false,
		'error' => isset($error) ? $error : false,
		'template' => isset($html) ? $html : false
	], 200);
});

/**
* Render page with form for check store by GET
*/
$app->get('/producers/edit', function ($request, $response, $arguments) {
	return $this->renderer->render($response, '/producers.edit.html', $arguments)
		->withHeader('Content-Type', CONTENT_TYPE_HTML);
});

/**
 * Render page with form for update producer by PUT
 */
$app->get('/producers/editor', function ($request, $response, $arguments) {
	return $this->renderer->render($response, '/producers.editor.html', $arguments)
		->withHeader('Content-Type', CONTENT_TYPE_HTML);
});

/**
 * Check exist store by id in GET
 */
$app->get('/producers/exist', function ($request, $response, $arguments) {
	$id = $request->getParam('id');
	if($id != null && !empty($id)) {
		$res = DB::get("SELECT id FROM `store` WHERE id = " . $id . " AND active = 1;");
		if($res->empty)
			$error = 'Элемент не найден';
	}

	return $response->withJson([
		'success' => isset($res) && $res->success && !$res->empty ? $res->success : false,
		'link'	=> isset($res) && !$res->empty ? '/stores/editor?id=' . $id : false,
		'error' => isset($error) ? $error : false,
	], 200);
});

$app->get('/producers/editor/load', function ($request, $response, $arguments) {
	$id = $request->getParam('id');
	if($id != null && !empty($id))
		$res = DB::get("SELECT * FROM `producer` WHERE id = " . $id . " AND active = 1 LIMIT 1;");

	if(isset($res) && $res->success && !$res->empty)
		$item = (array)$res->data[0];

	return $response->withJson([
		'success' => isset($res) && $res->success ? true : false,
		'error' => isset($error) ? $error : false,
		'item' => isset($res) && !empty($item) && is_array($item) ? $item : false
	], 200);
});

/**
 * Update info about store by id in PUT
 */
$app->put('/producers', function ($request, $response)  {
	$put = $request->getParsedBody(); //parse PUT data}

	if((!isset($put['name']) || empty($put['name'])) ||
		(!isset($put['id']) || empty($put['id'])))
		$error = 'Не все обязательные параметры были переданы.';

	if(!isset($error))
		$success = DB::exec('producer.update', [
			$put['name'],
			isset($put['guarantee']) && !empty($put['guarantee']) ? $put['guarantee'] : null,
			$put['id']
		])->success;

	return $response->withJson([
		'success' => isset($success) ? $success : false,
		'error' => isset($error) ? $error : false,
	], 200);
});

/**
 * Delete store by id in DELETE
 */
$app->delete('/producers', function ($request, $response)  {
	$delete = $request->getParsedBody(); //parse DELETE data

	if(isset($delete['id']) && !empty($delete['id']))
		$check = DB::get("SELECT * FROM `producer` WHERE id = " . $delete['id'] . " AND active = 1;");

	if(isset($check)){
		$sql = "SELECT id FROM `product` WHERE producer = " . $delete['id'] . " ;";
		$_res = DB::get($sql);

		if($_res->success && !empty($_res->data))
			foreach ($_res->data as $idx => $item)
				if(isset($item) && !empty($item) && is_array($item))
					DB::exec('product.delete', [
						$item['id']
					]);

		$success = DB::exec('producer.delete', [
			$delete['id']
		])->success;
	}else
		$error = 'Элемент не найден!';

	return $response->withJson([
		'success' => isset($success) ? $success : false,
		'error' => isset($error) ? $error : false
	], 200);
});

/*
 * Stores
 */

/**
 * List of stores by GET
 */
$app->get('/stores', function ($request, $response) {
	$items = [];
	$res = DB::get("SELECT id, name, address FROM `store` WHERE active = 1;");
	if(!$res->empty){
		foreach ($res->data as $item){
			if(!isset($item['id']) || empty($item['id']) || !isset($item['name']) || empty($item['name']))
				continue;
			$items[$item['id']] = $item['name'];

			if(isset($item['address']) && !empty($item['address']))
				$items[$item['id']] .= ' (' . $item['address'] . ')';
		}
	}

	if(!empty($items)){
		$html = '<div class="items">';
		foreach ($items as $key => $value)
			$html .= '<a class="item" href="/stores/editor?id=' . $key . '">' . $value . '</a>';
		$html .= '</div>';
	}

	return $response->withJson([
		'success' => isset($res) && $res->success ? $res->success : false,
		'items'	=> $res->empty == false ? $items : false,
		'error' => isset($error) ? $error : false,
		'res' => $res,
		'template' => isset($html) ? $html : false
	], 200);
});

$app->get('/stores/list', function ($request, $response, $arguments) {
	return $this->renderer->render($response, '/stores.list.html', $arguments)
		->withHeader('Content-Type', CONTENT_TYPE_HTML);
});

/**
 * Search of stores by GET
 */
$app->get('/stores/search', function ($request, $response) {
	$query = $request->getParam('query');
	if($query != null && !empty($query)){
		if(strlen($query) > 3){

			$items = [];
			$res = DB::get("SELECT id, name, address FROM `store` WHERE MATCH(name, address, phone, administrator) AGAINST ('" . Utils::clearSQL($query) . "') AND active = 1;");
			if(!$res->empty)
				foreach ($res->data as $item)
					if(isset($item['id']) && !empty($item['id']) && isset($item['name']) && !empty($item['name']))
						$items[$item['id']] = $item['name'] . (isset($item['address']) && !empty($item['address']) ? (' ('. $item['address'] . ')') : '');

			if(!empty($items)){
				$html = '<div class="items">';
				foreach ($items as $key => $value)
					$html .= '<a class="item" href="/stores/editor?id=' . $key . '">' . $value . '</a>';
				$html .= '</div>';
			}
		}
		else $error = 'Строка поиска должна быть от 4-ех символов';

	}else $error = 'Не азадана строка поиска.';

	return $response->withJson([
		'success' => isset($res) && $res->success ? $res->success : false,
		'items'	=> isset($res) && !$res->empty ? $items : false,
		'error' => isset($error) ? $error : false,
		'template' => isset($html) ? $html : false
	], 200);
});

/**
 * Render page with form for create store by POST
 */
$app->get('/stores/create', function ($request, $response, $arguments) {
	return $this->renderer->render($response, '/stores.create.html', $arguments)
		->withHeader('Content-Type', CONTENT_TYPE_HTML);
});

/**
 * Create store by POST
 */
$app->post('/stores', function ($request, $response) {
	$post = $request->getParsedBody(); //parse POST data

	if((!isset($post['name']) || empty($post['name'])) ||
		(!isset($post['administrator']) || empty($post['administrator'])))
		$error = 'Не все обязательные параметры были переданы.';

	if(!isset($error))
		$success = DB::exec('store.create', [
			$post['name'],
			$post['administrator'],
			isset($post['address']) && !empty($post['address']) ? $post['address'] : '',
			isset($post['phone']) && !empty($post['phone']) ? $post['phone'] : '',
			isset($post['start_time']) && !empty($post['start_time']) ? $post['start_time'] : '',
			isset($post['stop_time']) && !empty($post['stop_time']) ? $post['stop_time'] : '',
		])->success;

	return $response->withJson([
		'success' => isset($success) ? $success : false,
		'error' => isset($error) ? $error : false,
		'message' => isset($success) && !isset($error) ? 'Магазин зарегистрирован в базе данных.' : false
	], 200);
});

/**
 * Render page with form for check store by GET
 */
$app->get('/stores/edit', function ($request, $response, $arguments) {
	return $this->renderer->render($response, '/stores.edit.html', $arguments)
		->withHeader('Content-Type', CONTENT_TYPE_HTML);
});

/**
 * Render page with form for update store by PUT
 */
$app->get('/stores/editor', function ($request, $response, $arguments) {
	return $this->renderer->render($response, '/stores.editor.html', $arguments)
		->withHeader('Content-Type', CONTENT_TYPE_HTML);
});

/**
 * Check exist store by id in GET
 */
$app->get('/stores/exist', function ($request, $response, $arguments) {
	$id = $request->getParam('id');
	if($id != null && !empty($id)) {
		$res = DB::get("SELECT id FROM `store` WHERE id = " . $id . " AND active = 1;");
		if($res->empty)
			$error = 'Элемент не найден';
	}

	return $response->withJson([
		'success' => isset($res) && $res->success && !$res->empty ? $res->success : false,
		'link'	=> isset($res) && !$res->empty ? '/stores/editor?id=' . $id : false,
		'error' => isset($error) ? $error : false,
	], 200);
});

/**
 * Load data about store by id in GET
 */
$app->get('/stores/editor/load', function ($request, $response, $arguments) {
	$id = $request->getParam('id');
	if($id != null && !empty($id))
		$res = DB::get("SELECT * FROM `store` WHERE id = " . $id . " AND active = 1 LIMIT 1;");

	if(isset($res) && $res->success && !$res->empty)
		$item = (array)$res->data[0];


	return $response->withJson([
		'success' => isset($res) && $res->success ? true : false,
		'error' => isset($error) ? $error : false,
		'item' => isset($res) && !empty($item) && is_array($item) ? $item : false
	], 200);
});

/**
 * Update info about store by id in PUT
 */
$app->put('/stores', function ($request, $response)  {
	$put = $request->getParsedBody(); //parse PUT data}

	if((!isset($put['name']) || empty($put['name'])) ||
		(!isset($put['administrator']) || empty($put['administrator'])) ||
		(!isset($put['id']) || empty($put['id'])))
		$error = 'Не все обязательные параметры были переданы.';

	if(!isset($error))
		$success = DB::exec('store.update', [
			$put['name'],
			$put['administrator'],
			isset($put['address']) && !empty($put['address']) ? $put['address'] : null,
			isset($put['phone']) && !empty($put['phone']) ? $put['phone'] : null,
			isset($put['start_time']) && !empty($put['start_time']) ? $put['start_time'] : null,
			isset($put['stop_time']) && !empty($put['stop_time']) ? $put['stop_time'] : null,
			$put['id']
		])->success;

	return $response->withJson([
		'success' => isset($success) ? $success : false,
		'error' => isset($error) ? $error : false,
	], 200);
});

/**
 * Delete store by id in DELETE
 */
$app->delete('/stores', function ($request, $response)  {
	$delete = $request->getParsedBody(); //parse DELETE data

	if(isset($delete['id']) && !empty($delete['id']))
		$check = DB::get("SELECT * FROM `store` WHERE id = " . $delete['id'] . " AND active = 1;");

	if(isset($check)){
		$sql = "SELECT id, stores" .
			" FROM `product`" .
			" WHERE JSON_KEYS(JSON_EXTRACT(stores, '$[0]')) LIKE '%\"" . $delete['id'] . "\"%'";

		$_res = DB::get($sql);
		if($_res->success && !empty($_res->data)){
			foreach ($_res->data as $idx => $item){
				if(isset($item) && !empty($item) && is_array($item)){
					$id = $item['id'];
					$stores = json_decode($item['stores'])[0];
					$stores = (array)$stores;

					$keys = array_keys($stores);
					$values = array_values($stores);
					$skip_index = -1;
					$new_counts = [];
					foreach ($keys as $i => $k){
						$new_counts[] = $values[$i];
						if($k == $delete['id'])
							$skip_index = $i;
					}

					$new_stores = [];
					foreach ($keys as $i => $key)
						if($i != $skip_index)
							$new_stores[$key] = $new_counts[$i];
					$new_stores = [$new_stores];

					DB::exec('product.update_counts', [
						json_encode($new_stores),
						$id
					]);
				}
			}
		}

		$success = DB::exec('store.delete', [
			$delete['id']
		])->success;
	}else
		$error = 'Элемент не найден!';

	return $response->withJson([
		'success' => isset($success) ? $success : false,
		'error' => isset($error) ? $error : false
	], 200);
});

/*
 * Order (check)
 */

/**
 * Load products list by products array and store_id in GET
 */
$app->get('/order/products', function ($request, $response) {
	$products = $request->getParam('products');
	$store = $request->getParam('store');

	if($store != null && !empty($store) && (int)$store > 0){
		if($products != null && !empty($products) && is_array($products)){
			$sql = "SELECT id, article, name, weight, length, width, volume, cost, days_wait, stores," .
				" (SELECT name FROM `producer` WHERE id = producer) AS producer" .
				" FROM `product`" .
				" WHERE JSON_KEYS(JSON_EXTRACT(stores, '$[0]')) LIKE '%\"" . $store . "\"%'" .
				" AND ( ";

			$_products = [];
			foreach ($products as $id)
				$_products[] = ' id = ' . $id;
			$sql .= implode(' OR ', $_products);

			$res = DB::get($sql . ' )');

			if(!$res->empty){
				$html = '<div class="items">';
				foreach ($res->data as $key => $item){
					$stores = (array)json_decode($item['stores'])[0];

					$keys = array_keys($stores);
					$values = array_values($stores);
					foreach ($keys as $i => $k){
						if($k == $store){
							$item['count'] = $values[$i];
							break;
						}
					}

					$html .= '<a class="item item-collapse" data-id="' . $item['id'] . '" href="/products/editor?id=' . $item['id'] . '">' . $item['name'] .
					'<span data-id="' . $item['id'] . '" class="from_order">Отмена</span>' .
					'<span data-id="' . $item['id'] . '" class="collapse_order">Подробнее</span></a>' .
					'<div data-id="' . $item['id'] . '" class="collapse hidden">'.
						'<table class="table"><tbody>'.
						'<tr><td>Артикул</td><td>' . $item['article'] . '</td></tr>'.
						'<tr><td>Вес</td><td>' . $item['weight'] . '</td></tr>'.
						'<tr><td>Ширина</td><td>' . $item['width'] . '</td></tr>'.
						'<tr><td>Высота</td><td>' . $item['length'] . '</td></tr>'.
						'<tr><td>Объем</td><td>' . $item['volume'] . '</td></tr>'.
						'<tr><td>Дней под заказ</td><td>' . $item['days_wait'] . '</td></tr>'.
						'<tr><td>Производитель</td><td>' . $item['producer'] . '</td></tr>'.
						'</tbody></table>' .
						'<table class="table"><tbody>'.
						'<tr><td>Стоимость</td><td class="cost" data-id="' . $item['id'] . '">' . $item['cost'] . '</td></tr>'.
						'<tr><td>Количество на складе</td><td>' . (isset($item['count']) ? $item['count'] : 0) . '</td></tr>'.
						'<tr rowspan="2"><td>Количество</td>'.
						'<td><input min="0" max="' . (isset($item['count']) ? $item['count'] : 0) . '" type="number" value="0" class="text ui-widget-content ui-corner-all input-count" placeholder="Введите число" data-id="' . $item['id'] . '"/></td></tr>'.
						'<tr><td>Итого</td><td class="sum" data-id="' . $item['id'] . '"></td></tr>'.
						'</tr>'.
						'</tbody></table>' .
					'</div><div class="clearfix"></div>';
				}
				$html .= '</div>';
			}
		}else $error = 'Не заданы товары.';
	}else $error = 'Не задан магазин.';


	return $response->withJson([
		'success' => isset($res) && $res->success ? $res->success : false,
		'error' => isset($error) ? $error : false,
		'res' => isset($res) && !empty($res) ? $res : false,
		'template' => isset($html) && !empty($html) ? $html : false,
		'params' => [$store, $products]
	], 200);
});

/**
 * Render page with form for create order by POST
 */
$app->get('/order', function ($request, $response, $arguments) {
	return $this->renderer->render($response, '/order.html', $arguments)
		->withHeader('Content-Type', CONTENT_TYPE_HTML);
});

/**
 * Render page with form for check orders by GET
 */
$app->get('/orders', function ($request, $response, $arguments) {
	return $this->renderer->render($response, '/orders.html', $arguments)
		->withHeader('Content-Type', CONTENT_TYPE_HTML);
});

/**
 * Check exist order by id in GET
 */
$app->get('/order/exist', function ($request, $response, $arguments) {
	$id = $request->getParam('id');
	if($id != null && !empty($id)) {
		$res = DB::get("SELECT id FROM `_check` WHERE id = " . $id . " AND active = 1;");
		if($res->empty)
			$error = 'Элемент не найден';
	}

	return $response->withJson([
		'success' => isset($res) && $res->success && !$res->empty ? $res->success : false,
		'link'	=> isset($res) && !$res->empty ? '/order/print?id=' . $id . '&print=y' : false,
		'error' => isset($error) ? $error : false,
	], 200);
});

/**
 * Create order by POST
 */
$app->post('/order', function ($request, $response) {
	$post = $request->getParsedBody(); //parse POST data

	$products = [];
	if(isset($post['products']) && isset($post['counts'])){
		if(is_array($post['products']) && is_array($post['counts'])){
			if(!empty($post['products']) && !empty($post['counts'])){
				if(count($post['products']) == count($post['counts'])){
					$length = count($post['products']);
					$_products = [];
					for($i = 0; $i < $length; $i++)
						$_products[] = [$post['products'][$i] => $post['counts'][$i]];

					array_walk_recursive($_products, function($item, $key) use (&$products){
						$products[$key] = isset($products[$key]) ?  $item + $products[$key] : $item;
					});
					$products = [$products];
				}
			}
		}
	}

	if(!isset($post['store']) || empty($post['store']) || !((int)$post['store'] > 0) ||
		!isset($products) || empty($products) || !is_array($products))
		$error = 'Не все обязательные параметры были переданы.';

	if(!isset($error)){
		$store = $post['store'];
		$res = DB::exec('check.create', [
			json_encode($products),
			$store
		], true);
		if($res->success){
			$sql = "SELECT id, stores" .
				" FROM `product`" .
				" WHERE JSON_KEYS(JSON_EXTRACT(stores, '$[0]')) LIKE '%\"" . $store . "\"%'" .
				" AND ";

			$_products = [];
			foreach ($post['products'] as $id)
				$_products[] = ' id = ' . $id;
			$sql .= implode(' OR ', $_products);

			$_res = DB::get($sql);
			foreach ($_res->data as $idx => $item){
				if(isset($item) && !empty($item) && is_array($item)){
					$id = $item['id'];
					$stores = json_decode($item['stores'])[0];
					$stores = (array)$stores;

					$keys = array_keys($stores);
					$values = array_values($stores);
					$new_counts = [];
					foreach ($keys as $i => $k){
						if($k == $store){
							foreach ($post['products'] as $j => $_id)
								if($_id == $id)
									$new_counts[] = (string)($values[$i] - $post['counts'][$j]);
						}
						else
							$new_counts[] = $values[$i];
					}

					$new_stores = [];
					foreach ($keys as $i => $key)
						$new_stores[$key] = $new_counts[$i];
					$new_stores = [$new_stores];

					DB::exec('product.update_counts', [
						json_encode($new_stores),
						$id
					]);
				}
			}
			$order_id = $res->insert_id;
		}else $error = $res->error;
	}

	return $response->withJson([
		'success' => isset($res) && $res->success ? $res->success : false,
		'error' => isset($error) ? $error : false,
		'order_id' => isset($order_id) ? $order_id : false,
		'res' => isset($res) ? $res : false
	], 200);
});

/**
 * Render page with template for PDF Browser Print
 */
$app->get('/order/print', function ($request, $response) {
	return $this->renderer->render($response, '/order.print.html', [])
		->withHeader('Content-Type', CONTENT_TYPE_HTML);
});

/**
 * Load data abut order by id in GET
 */
$app->get('/order/print/load', function ($request, $response) {
	$id = (int)$request->getParam('id');

	if(isset($id) && !empty($id) && ($id > 0)){
		$res = DB::get('SELECT date, products, (SELECT name FROM `store` WHERE  id = _check.store) as store, (SELECT administrator FROM `store` WHERE  id = _check.store) as admin, (SELECT address FROM `store` WHERE  id = _check.store) as address, (SELECT phone FROM `store` WHERE  id = _check.store) as phone FROM `_check` WHERE id = ' . $id . ' LIMIT 1;');
		if($res->success && !$res->empty){
			$item = $res->data[0];

			$products = json_decode($item['products'])[0];
			$products = (array)$products;

			$IDs = array_keys($products);
			foreach ($IDs as $i => $id)
				$IDs[$i] = ' id = ' . $id;

			$counts = array_values($products);

			$sql = 'SELECT article, name, cost FROM `product` WHERE ';
			$sql .= implode(' OR ', $IDs);

			$_res = DB::get($sql);
			if($_res->success && !$_res->empty){
				$item['items'] = $_res->data;
				$html = '';
				$total = 0;
				foreach ($item['items'] as $i => $_item){
					$count = (int)$counts[$i];
					$cost = (float)$_item['cost'];
					$total += (float)($count * $cost);
					$html .= '<tr>'.
						'<td>' . (isset($_item['article']) && !empty($_item['article']) ? $_item['article'] : ' &nbsp; ') . '</td>'.
						'<td>' . $_item['name'] . '</td>'.
						'<td>' . $count . '</td>'.
						'<td>' . $cost . '</td>'.
						'</tr>';
				}
				$settings = [
					'store' => isset($item['store']) && !empty($item['store']) ? $item['store'] : false,
					'admin' => isset($item['admin']) && !empty($item['admin']) ? $item['admin'] : false,
					'phone' => isset($item['phone']) && !empty($item['phone']) ? $item['phone'] : false,
					'address' => isset($item['address']) && !empty($item['address']) ? $item['address'] : false,
					'datetime' => isset($item['date']) && !empty($item['date']) ? $item['date'] : false,
					'total' => isset($total) && !empty($total) ? $total : false,
				];
			}else $error = 'Не удалось получить список товаров.';
		}else $error = 'Элемент не найден.';
	}else $error = 'Не задан обязательный параметр!';

	return $response->withJson([
		'success' => isset($res) && $res->success ? $res->success : false,
		'error' => isset($error) ? $error : false,
		'order_id' => isset($order_id) ? $order_id : false,
		'res' => isset($res) ? $res : false,
		'$_res' => isset($_res) ? $_res : false,
		'template' => isset($html) ? $html : false,
		'settings' => isset($settings) ? $settings : false
	], 200);
});

/*
 * Other
 */

/**
 * Check connection with MySQL database
 */
$app->get('/db/connection/ping', function ($request, $response) {
	$res = DB::ping();
	return $response->withJson([
		'success' => isset($res) && $res->success ? $res->success : false,
	], 200);
});
