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

use Slim\Views\PhpRenderer;

/**
 * Dev display errors
 */
error_reporting(E_ALL);
ini_set('display_errors', 1);

/**
 * Autoload vendor libs
 */
require_once '../vendor/autoload.php';

/**
 * Init application static const and global vars
 */
require_once '../app/header.php';

/**
 * Init Slim Application
 *
 * @var array $config
 */
$app = new \Slim\App(['settings' => $config]);

//settings for public folder
$container = $app->getContainer();
$container['renderer'] = new PhpRenderer('../public/');

/**
 * Init application classes
 */
require_once '../app/classes.php';

/**
 * Init application routes
 */
require_once '../app/routes.php';

/**
 * Run Slim Application
 */
$app->run();