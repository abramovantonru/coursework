console.time('main');
const
	title = 'Сеть мебельных магазинов',
	paths = {
	api: {
		main: '/api',
		producers:{
			main:  '/producers'
		},
		products: '/products'
	}
};

$(document).ready(function () {
	(function () {
		var App = (function () {
				return{
					init: function () {
						SEO.init();
						Menu.init();
						Alert.init();
						Form.init();
					}
				}
			})(),
			Utils = (function () {
				var
					i = 0, j = 0, k = 0,
					length = 0;
				return{
					queryString: function() {
						var string = window.location.search.substr(1).split('&');
						length = string.length;
						if (!length) return {};

						var params = {};
						for (i = 0; i < length; i++) {
							var param = string[i].split('=', 2);
							if (param.length != 2) continue;
							params[param[0]] = decodeURIComponent(param[1].replace(/\+/g, ' '));
						}

						return params;
					},
					getPath: function (args) {
						var
							length = args.length,
							i = 0, path = '', child = paths;
						if(!length) return false;
						for(i = 0; i < length; i++){
							if(typeof child !== 'undefined'){
								child = child[args[i]];
								if(typeof child !== 'undefined')
									if(typeof child.main !== 'undefined' && child.main.length)
										path += child.main;
									else
										path += child;
							}
						}
						return path;
					},
					secureGetPath: function (string) {
						var response = false;

						try {
							var arr = string.split('=')[1].split(',');
							response = Utils.getPath(arr);
						}catch (e){
							console.log(e);
							response = false;
						}

						return response;
					},
					loadSelects: function ($form) {
						$form.find('select[data-loadSelect="true"]').each(function (idx, element) {
							var url = $(element).attr('data-url');
							if(url.indexOf('getPath') > -1){
								var path = Utils.secureGetPath(url);

								if(path && path.length)
									$(element).attr('data-url', path);
							}
							if(!url.length) return true;
							$(element)
								.attr('disable', true)
								.prepend('<option value="" selected="selected">Загрузка списка...</option>');

							var
								html = '',
								items = [{key: 'Элемент1', value: '1'}, {key: 'Элемент2', value: '2'}];
							length = items.length;
							if(length)
								html = Utils.selectFromArray(items);
							$(element).append(html);
							$(element)
								.attr('disable', true)
								.find('option').first().remove();

							/*$.ajax({
							 url: url,
							 type: 'GET',
							 dataType: 'json',
							 success: function (res) {
							 if(res.success && res.items){
							 var
							 html = '',
							 items = res.items;
							 length = items.length;
							 if(length)
							 html = Utils.selectFromArray(items);
							 $(element).append(html);
							 $(element)
							 .attr('disable', true)
							 .find('option').first().remove();
							 }
							 },
							 error: function (res) {

							 }
							 });*/
						});
					},
					selectFromArray: function (data) {
						if(typeof data === 'undefined' || !data.length) return false;
						var html = '';
						length = data.length;
						for(i = 0; i < length; i++)
							if(data[i].value && data[i].key)
								html += '<option value="' + data[i].value + '">' + data[i].key + '</option>';
						return html;
					}
				}
			})(),
			Alert = (function () {
				var
					$alerts = undefined,
					$success = undefined,
					$error = undefined,
					$info = undefined,
					$loadAlerts = undefined;
				return{
					init: function () {
						$alerts = $('#alerts');
						if(!$alerts.length) return false;

						$success = $alerts.find('.alert-success');
						$error = $alerts.find('.alert-error');
						$info = $alerts.find('.alert-info');
						$loadAlerts = $('.load-alert');

						if($loadAlerts.length)
							Alert.loadAlerts();
					},
					success: function (text) {
						if(typeof $success === 'undefined') return false;
						$success.find('.text').html(text);
						return $success[0].outerHTML;
					},
					error: function (text) {
						if(typeof $error === 'undefined') return false;
						$error.find('.text').html(text);
						return $error[0].outerHTML;
					},
					info: function (text) {
						if(typeof $info === 'undefined') return false;
						$info.find('.text').html(text);
						return $info[0].outerHTML;
					},
					loadAlerts: function () {
						$loadAlerts.each(function (idx, element) {
							if(!$(element).hasClass('laoded')){
								var
									html = $(element).html(),
									type = $(element).attr('data-type');
								if(html && html.length && type && type.length){
									try {
										$(element)
											.html(Alert[type](html))
											.addClass('loaded');
									}catch (e){
										console.error(e);
									}
								}
							}
						})
					}
				}
			})(),
			Form = (function () {
				var
					$form = undefined,
					$response = undefined,
					$progressBar = undefined,

					$enableBtn = undefined,

					$magazine = undefined,
					$counts = undefined,

					$doubleInputs = undefined,
					$integerInputs = undefined;
				return{
					init: function () {
						$form = $('#send_form');
						if(!$form.length) return false;
						Form.loadUrl();

						$response = $form.find('.response');
						$progressBar = $form.find('.progress_bar');

						$enableBtn = $form.find('.form-enable');

						$magazine = $('#form_magazine');
						$counts = $('#form_counts');

						$doubleInputs = $form.find('.only_double');
						$integerInputs = $form.find('.only_integer');

						if(typeof $progressBar !== 'undefined')
							$progressBar.progressbar({
								max: 100,
								value: 0
							});

						Utils.loadSelects($form);

						Form.events();

						if($form.hasClass('disable'))
							Form.disable();
					},
					disable: function () {
						$form.find('input, select').each(function (idx, element) {
							$(element).attr('disabled', true);
						});

						$form.find('button:not(.form-enable)').attr('disabled', true);
					},
					enable: function () {
						$form.find('input, select').each(function (idx, element) {
							$(element).attr('disabled', false);
						});

						$form.find('button').attr('disabled', false);
						$enableBtn.remove();
					},
					events: function () {
						if($form.length)
							$form
								.unbind('submit.sendData')
								.bind('submit.sendData', function (e) {
									e.preventDefault();
									if(!Form.validation(this)) return false;
									Form.send(this);
									return false;
								});

						if($doubleInputs.length)
							$doubleInputs
								.unbind('input.onlyDouble')
								.bind('input.onlyDouble', function (e) {
									e.preventDefault();
									this.value = this.value.replace(/[^0-9.]/g, '').replace(/(\..*)\./g, '$1');
									return false;
								});

						if($integerInputs.length)
							$integerInputs
								.unbind('input.onlyInteger')
								.bind('input.onlyInteger', function (e) {
									e.preventDefault();
									this.value = this.value.replace(/[^0-9\.]/g,'');
									return false;
								});

						if($enableBtn.length)
							$enableBtn
								.unbind('click.enableForm')
								.bind('click.enableForm', function (e) {
									e.preventDefault();
									Form.enable();
									return false;
								});

						if($magazine.length){
							$('#form_magazine_add')
								.unbind('click.add')
								.bind('click.add', function (e) {
									e.preventDefault();
									var $clone = $magazine.find('select').first().clone();

									$clone
										.attr('value', '');
									$(this).before($clone[0].outerHTML);

									$clone = $counts.find('input').first().clone();
									$clone
										.attr('value', '');
									$counts.append($clone[0].outerHTML);

									return false;
								});

							$('#form_magazine_remove')
								.unbind('click.remove')
								.bind('click.remove', function (e) {
									e.preventDefault();
									var
										$elements = $magazine.find('select'),
										idx = $elements.last().index();

									if(idx){
										$elements.eq(idx).remove();
										$elements = $counts.find('input');
										$elements.eq(idx).remove();
									}else{
										$magazine.find('select').eq(0)
											.val('');
										$counts.find('input').eq(0)
											.val('');
									}

									return false;
								});
						}
					},
					validation: function (form) {
						var error = '';
						$(form).find('*').each(function (idx, element) {
							if($(element).attr('data-valid') === 'novalid') return true;

							var name = $('label[for="' + $(element).attr('id') + '"]').text();

							if($(element).attr('required') === 'required' && $(element).val() == '')
								error = 'Не заполнено обязательное поле "' + name + '"!';
						});
						if(error.length){
							console.error(error);
							return false;
						}

						return true;
					},
					send: function (that) {
						var
							url = $(that).attr('data-url'),
							method = $(that).attr('data-method'),
							data = new FormData(that),
							xhr = function () { return $.ajaxSettings.xhr(); };

						if($progressBar.length){
							xhr = function(){
								var xhr = $.ajaxSettings.xhr();
								xhr.upload.addEventListener('progress', function(e){
									if(e.lengthComputable) {
										var progress = Math.ceil(e.loaded / e.total * 100);
										$progressBar.progressbar('value', progress);
									}
								}, false);
								return xhr;
							};
						}

						$.ajax({
							url: url,
							method: method,
							data: data,
							processData: false,
							contentType: 'multipart/form-data',
							dataType: 'json',
							cache: false,
							xhr: xhr,
							beforeSend: function () {
								$response.html('');
							},
							success: function (res) {
								console.log(res);
							},
							error: function (res) {
								if(res.status && res.statusText && res.statusText.length)
									$response.html(Alert.error('Ответ сервера: ' + res.statusText + ' [<b>' + res.status + '</b>]'));
								console.log(res);
							}
						});
					},
					loadUrl: function () {
						var url = Utils.secureGetPath($form.attr('data-url'));
						if(url && url.length)
							$form.attr('data-url', url);
						else
							$form.prepend(Alert.error('Не удалось получить ссылку формы.'));
					}
				}
			})(),
			Menu = (function () {
				var
					$menu = undefined,
					$submenu = undefined,
					$menu_item = undefined,

					items = [
						{name: 'Заказ<span id="order_items_count" class="badge">0</span>', link: '/order'},
						{name: 'Покупки', link: [
							{name: 'Список', link: '/orders.list.html'},
							{name: 'Создать', link: '/orders.create.html'},
							{name: 'Редактировать', link: '/orders.edit.html'},
							{name: 'Отчеты', link: '/orders.reviews.html'}
						]},
						{name: 'Товары', link: [
							{name: 'Список', link: '/products.list.html'},
							{name: 'Создать', link: '/products.create.html'},
							{name: 'Редактировать', link: '/products.edit.html'}
						]},
						{name: 'Магазины', link: [
							{name: 'Список', link: '/stores.list.html'},
							{name: 'Создать', link: '/stores.create.html'},
							{name: 'Редактировать', link: '/stores.edit.html'},
							{name: 'Отчеты', link: '/stores.reviews.html'}
						]},
						{name: 'Производители', link: [
							{name: 'Список', link: '/stores.list.html'},
							{name: 'Создать', link: '/stores.create.html'},
							{name: 'Редактировать', link: '/stores.edit.html'},
							{name: 'Отчеты', link: '/stores.reviews.html'}
						]}
					],
					submenu = [],

					i = 0, j = 0,
					length = 0,

					html = '',
					height = window.innerHeight;
				return{
					init: function () {
						length = items.length;
						$menu = $('#menu');

						if(!length || !$menu.length) return false;

						$submenu = $('#submenu');

						for(i = 0; i < length; i++){
							var
								item = items[i],
								type = typeof item.link;
							if(type === 'object'){
								var
									subItems = item.link,
									lenSubItems = subItems.length;

								submenu[i] = [];
								for(j = 0; j < lenSubItems; j++)
									submenu[i].push({name: subItems[j].name, link: subItems[j].link});

								html += '<a href="#" class="menu-item parent">' + item.name + '</a>';
							}else if(type === 'string' && item.link != '')
								html += '<a href="' + item.link + '" class="menu-item">' + item.name + '</a>';
						}

						if(html.length)
							$menu.html($.parseHTML(html));

						$menu_item = $('.menu-item');

						Menu.events();
					},
					events: function () {
						$menu.resizable({
							minWidth: 120,
							maxWidth: 300,
							handles: 'e',
							resize: function( event, ui ) {
								ui.size.height = height;
								$submenu.css('left', ui.size.width + 'px');
							}
						});

						$menu
							.unbind('click.toggleMenu')
							.bind('click.toggleMenu', function (e) {
								e.preventDefault();
								if (e.offsetX > $menu[0].offsetWidth)
									Menu.toggle();
								return false;
							});

						$menu_item
							.unbind('click.selectItem')
							.bind('click.selectItem', function (e) {
								e.preventDefault();

								var url = $(this).attr('href');
								if(url == '#'){
									var idx = $('.menu-item').index(this);
									Menu.submenu.init(idx);
									Menu.submenu.show();
								}else window.location = url;

								return false;
							});
					},
					submenu:{
						init: function(idx){
							var html = '';

							length = submenu[idx].length;
							for(i = 0; i < length; i++)
								html += '<a href="' + submenu[idx][i].link + '" class="menu-item">' + submenu[idx][i].name + '</a>';

							$submenu.html(html);
						},
						show: function () {
							$submenu
								.addClass('open')
								.css('display', 'block')
								.css('left', $menu.width())
								.resizable({
									minWidth: 100,
									maxWidth: 300,
									handles: 'e',
									resize: function( event, ui ) {
										ui.size.height = height;
									}
								});
						},
						hide: function () {
							$submenu
								.removeClass('open')
								.css('display', 'none');
							if($submenu.resizable('instance'))
								$submenu.resizable('destroy');
						}
					},
					show: function () {
						$menu.addClass('open');
						$menu.css('left', '0');
						$menu.resizable('option', 'disabled', false);
					},
					hide: function () {
						$menu.css('left', '-' + $menu.width() + 'px');
						$menu.removeClass('open');
						$menu.resizable('option', 'disabled', true);
						Menu.submenu.hide();
					},
					toggle: function () {
						if($menu.hasClass('open'))
							Menu.hide();
						else
							Menu.show();
					}
				}
			})(),
			SEO = (function () {
				var
					meta = undefined,
					$meta = undefined,

					length = 0,
					i = 0, j =0, k = 0;
				return{
					init: function () {
						$meta = $('.meta');
						
						if(!$meta.length) return false;
						meta = {};
						
						SEO.parseMeta(function () {
							if(typeof meta['section'] !== 'undefined' && meta['section'].length)
								window.section = meta['section'];

							if(typeof meta['page'] !== 'undefined' && meta['page'].length)
								window.page = meta['page'];

							SEO.setMeta();
						});
					},
					parseMeta: function (callback) {
						$meta.each(function (idx, element) {
							var
								key = $(element).attr('data-key'),
								value = $(element).attr('data-value');
							if(key && key.length && value && value.length)
								meta[key] = value;
						});

						callback();
					},
					setMeta: function () {
						window.title = title;

						$('#title').text(window.title);
						$('#page_title').html(window.title + ': ' + window.section + ' > ' + window.page);
					}
				}
			})();

		App.init();
		console.log(Utils.queryString());
	})();
});

console.timeEnd('main');
