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

/**
 * List of products by GET
 */
$app->get('/products', function ($request, $response) {
	$items = [];
	$res = DB::get("SELECT id, name FROM `product`;");
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
		'items'	=> $res->empty == false ? $items : false,
		'error' => isset($error) ? $error : false,
		'template' => $html,
	], 200);
});

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

$app->get('/products/editor/load', function ($request, $response, $arguments) {
	$id = $request->getParam('id');
	if($id != null && !empty($id))
		$res = DB::get("SELECT * FROM `product` WHERE id = " . $id . " AND active = 1;");

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
		'data' => isset($res) && isset($res->data) ? $res->data : false,
		'item' => $item
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
					$html .= '<a class="item" href="/products/edit?id=' . $key . '">' . $value . '<span data-id="' . $key . '" class="order_btn">В заказ</span></a>';
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
		]);

	return $response->withJson([
		'success' => isset($success) ? $success : false,
		'error' => isset($error) ? $error : false,
		'stores' => $stores
	], 200);
});

/**
 * Update info about product by PUT
 */
$app->put('/products', function ($request, $response)  {
	$put = $request->getParsedBody(); //parse PUT data
	$params = (array)json_decode(array_keys($put)[0]); // PUT data to array

	return $response->withJson([
		'params' => $params,
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

	return $response->withJson([
		'success' => isset($res) && $res->success ? $res->success : false,
		'items'	=> $res->empty == false ? $items : false,
		'error' => isset($error) ? $error : false,
		'res' => $res
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
		$res = DB::exec('producer.create', [
			$post['name'],
			(isset($post['guarantee']) && !empty($post['guarantee']) ? $post['guarantee'] : null)
		]);
		$error = $res->error;
	}

	return $response->withJson([
		'success' => isset($res) && $res->success ? $res->success : false,
		'error' => isset($error) ? $error : false,
		'res' => $res
	], 200);
});

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

	return $response->withJson([
		'success' => isset($res) && $res->success ? $res->success : false,
		'items'	=> $res->empty == false ? $items : false,
		'error' => isset($error) ? $error : false,
		'res' => $res
	], 200);
});

/**
 * Check connection with MySQL database
 */
$app->get('/db/connection/ping', function ($request, $response) {
	$res = DB::ping();
	return $response->withJson([
		'success' => isset($res) && $res->success ? $res->success : false,
	], 200);
});

