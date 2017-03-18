<?
/**
 * Abramov Anton 2017
 *
 * Application for connect client-side and database.
 * It's API service with GET, POST, PUT, DELETE methods (REST).
 *
 * Features:
 * - support all methods
 * - full work with database for client
 * - OOP style
 * - Slim (micro)Framework
 * -
 */

/**
 * Class DBResult
 * Model for work with database response data
 */
class DBResult{
	public $success = false; // response status
	public $empty = false; // empty marker
	public $data = []; // for data in rows and cols (array)
	public $error; // response error
}

/**
 * Class DB
 * It's wrapper for work with database connection and queries
 */
class DB{
	//database settings
	private $host;
	private $database;
	private $user;
	private $password;

	private $connection; // mysqli connection
	private $error; // for error of connection

	function __construct($_host = DB_HOST, $_db = DB_NAME, $_user = DB_USER, $_password = DB_PASSWORD){
		$this->host = $_host;
		$this->database = $_db;
		$this->user = $_user;
		$this->password = $_password;
	}

	private function connect(){
		$this->connection = new mysqli($this->host, $this->user, $this->password, $this->database);

		if($this->connection->connect_error)
			$this->error = 'Connect Error (' . $this->connection->connect_errno . ') ' . $this->connection->connect_error;
	}

	private function disconnect(){
		try{
			$this->connection->close();
		}catch (Exception $e){
			$this->error = $e->getMessage();
		}
	}

	public static function exec($name, $params){
		$db = new DB();
		$result  = new DBResult();
		$sql = 'CALL `' . $name . '`';

		$db->connect();

		if(!empty($params)){
			foreach ($params as $key => $value)
				if($value !== null)
					$params[$key] = "'" . mysqli_real_escape_string($db->connection, (string)$value) . "'";
				else
					$params[$key] = 'NULL';

			$sql .= "(" . implode(',', $params) . ")";
		}

		if(!$result->success = $db->connection->query($sql))
			$result->error = $db->connection->error;

		$result->data['sql'] = $sql;

		$db->disconnect();

		return $result;
	}

	public static function get($sql){
		$db = new DB();
		$result = new DBResult();

		$db->connect();

		//$sql = mysqli_real_escape_string($db->connection, $sql);
		//$result->data['sql'] = $sql;
		if(!$res = $db->connection->query($sql))
			$result->error = $db->connection->error;
		else{
			$result->success = true;

			while($row = $res->fetch_assoc())
				$result->data[] = $row;

			if(empty($result->data))
				$result->empty = true;
		}

		$db->disconnect();

		return $result;
	}

	public static function update($sql){
		$db = new DB();
		$result = new DBResult();

		$db->connect();

		if(!$res = $db->connection->query($sql))
			$db->error = $db->connection->error;
		else
			$result->success = true;

		$db->disconnect();

		return $result;
	}
}


class Utils{
	public static function clearSQL($string){
		return self::clear2Quotes($string);
	}

	public static function clear2Quotes($string){
		return str_replace('"', '\"', $string);
	}

	/*public static function clearQuotes($string){
		return str_replace("'", "\'", $string);
	}*/
}