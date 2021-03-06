<?
/**
 * Abramov Anton 2017
 * Classes
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
	public $insert_id; // last insert id
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

	/**
	 * DB constructor.
	 * @param string $_host
	 * @param string $_db
	 * @param string $_user
	 * @param string $_password
	 */
	function __construct($_host = DB_HOST, $_db = DB_NAME, $_user = DB_USER, $_password = DB_PASSWORD){
		$this->host = $_host;
		$this->database = $_db;
		$this->user = $_user;
		$this->password = $_password;
	}

	/**
	 * Open connection with database
	 */
	private function connect(){
		$this->connection = new mysqli($this->host, $this->user, $this->password, $this->database);

		if($this->connection->connect_error)
			$this->error = 'Connect Error (' . $this->connection->connect_errno . ') ' . $this->connection->connect_error;
	}

	/**
	 * Close connection with database
	 */
	private function disconnect(){
		try{
			$this->connection->close();
		}catch (Exception $e){
			$this->error = $e->getMessage();
		}
	}

	/**
	 * Wrapper for execute procedure of database
	 * @param $name
	 * @param $params
	 * @param bool $insert_id
	 * @return DBResult
	 */
	public static function exec($name, $params, $insert_id = false){
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
		else if($insert_id){
			$_id = $db->connection->query('SELECT LAST_INSERT_ID() as insert_id LIMIT 1;')->fetch_assoc();
			$result->insert_id = $_id['insert_id'];
		}

		$result->data['sql'] = $sql;

		$db->disconnect();

		return $result;
	}

	/**
	 * Wrapper for get data from database
	 * @param $sql
	 * @return DBResult
	 */
	public static function get($sql){
		$db = new DB();
		$result = new DBResult();

		$db->connect();

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
}

/**
 * Class Utils
 * Different utils
 */
class Utils{
	/**
	 * Replace symbols in SQL query string
	 * @param $string
	 * @return mixed
	 */
	public static function clearSQL($string){
		return self::clearQuotes(self::clear2Quotes($string));
	}

	public static function clear2Quotes($string){
		return str_replace('"', '\"', $string);
	}

	public static function clearQuotes($string){
		return str_replace("'", "\\'", $string);
	}
}