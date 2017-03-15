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
 * Render page with form for create product by POST
 */
$app->get('/products/create', function ($request, $response, $arguments) {
	return $this->renderer->render($response, '/products.create.html', $arguments)
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
			$html .= '<a class="item" href="/products/edit?id=' . $key . '">' . $value . '<span data-id="' . $key . '" class="order_btn">В заказ</span></a>';
		$html .= '</div>';
	}

	return $response->withJson([
		'success' => isset($res) && $res->success ? $res->success : false,
		'items'	=> $res->empty == false ? $items : false,
		'error' => isset($error) ? $error : false,
		'template' => $html,
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
			$res = DB::get("SELECT id, name FROM `product` WHERE MATCH(name, article) AGAINST ('" . Utils::clearSQL($query) . "');");
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
					for($i = 0; $i < $length; $i++)
						$stores[] = [$post['stores'][$i] => $post['counts'][$i]];
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
		$success = DB::exec('products.create', [
			isset($post['article']) ? $post['article'] : uniqid(),
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
		'error' => isset($error) ? $error : false
	], 200);
});

/**
 * List of producers by GET
 */
$app->get('/producers', function ($request, $response) {
	$items = [];
	$res = DB::get("SELECT * FROM `producer`;");
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
		$res = DB::exec('producers.create', [
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
	$res = DB::get("SELECT id, name, address FROM `store`;");
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



