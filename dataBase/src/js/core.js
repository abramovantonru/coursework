console.time('main');
const
	title = 'Сеть мебельных магазинов',
	paths = {
	stores: '/stores',
	producers: '/producers',
	products:{
		main: '/products',
		search: '/search',
		exist: '/exist'
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
						Order.init();

						ProductEditor.init();
						Utils.loadOpenEditor();
					}
				}
			})(),
			Utils = (function () {
				var
					i = 0, j = 0, k = 0,
					length = 0;
				return{
					createStoresData: function ($selects, $inputs, stores, counts) {
						console.log(stores);
						length = stores.length;
						if(length == counts.length){

							for(i = 0; i < length; i++){
								var _select = $selects.find('select').eq(0).clone();
								_select.children('option').each(function (idx, element) {
									if($(element).attr('value') == stores[i])
										$(element).attr('selected', true);
								});
								$selects.prepend(_select[0].outerHTML);
							}

							length = counts.length;
							for(i = 0; i < length; i++){
								var _input = $inputs.find('input').eq(0).clone();
								_input.attr('value', counts[i]);
								$inputs.prepend(_input[0].outerHTML);
							}
						}
					},
					fillDataToForm: function($form, data){
						if(typeof data === 'object')
							for(i in data)
								$form.find('[name="' + i + '"]').val(data[i]);
					},
					loadOpenEditor: function () {
						var $openEditor = $('#open_editor');
						if(!$openEditor.length) return false;

						var path = $openEditor.attr('data-url');
						if(path && path.length && path.indexOf('getPath') > -1){
							path = Utils.secureGetPath(path);
							if(path && path.length)
								$openEditor.attr('data-url', path);
							else
								$openEditor.find('.response').html(Alert.error('Ошибка: не удалось определить ссылку формы!'));
						}

						$openEditor
							.unbind('submit.exist')
							.bind('submit.exist', function (e) {
								e.preventDefault();
								var
									url = $(this).attr('data-url'),
									method = $(this).attr('data-method'),
									$response = $(this).find('.response'),
									data = new FormData(this);

								if(path && path.length)
									$.ajax({
										url: url,
										method: method,
										data: $(this).serialize(),
										processData: false,
										dataType: 'json',
										cache: false,
										beforeSend: function () {
											$response.html('');
										},
										success: function (res) {
											if(res.success && res.link)
												window.location = res.link;
											else if(res.error)
												$response.html(Alert.error(res.error));
										},
										error: function (res) {
											if(res.status && res.statusText && res.statusText.length)
												$response.html(Alert.error('Ответ сервера: ' + res.statusText + ' [<b>' + res.status + '</b>]'));
										}
									});
								return false;
							});
					},
					parseModuleNames: function (string) {
						var modules = [];
						try{
							modules = string.split(',');
						}catch (e){
							console.error(e);
						}
						return modules;
					},
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

								if(path && path.length){
									$(element).attr('data-url', path);
									url = path;
								}
							}
							if(!url.length) return true;

							$(element)
								.attr('disabled', true)
								.prepend('<option value="" selected="selected">Загрузка списка...</option>');

							$.ajax({
								url: url,
								type: 'GET',
								dataType: 'json',
								success: function (res) {
									if(res.success && res.items){
										var html = Utils.selectFromArray(res.items);
										$(element).append(html);
										$(element)
											.attr('disabled', false)
											.find('option').first().remove();
									}else{
										$(element)
											.attr('disabled', true)
											.html('<option value="" selected="selected">Ошибка! Не удалось загрузить список.</option>');
									}
								},
								error: function (res) {
									$(element)
										.attr('disabled', true)
										.html('<option value="" selected="selected">Ошибка! Не удалось загрузить список.</option>');
									console.error(res);
								}
							});
						});
					},
					selectFromArray: function (items) {
						if(typeof items === 'undefined') return false;
						var html = '';
						for(i in items)
							html += '<option value="' + i + '">' + items[i] + '</option>';
						return html;
					}
				}
			})(),
			ProductEditor = (function () {
				var
					$editor = undefined,
					$removeBtn = undefined,
					$response = undefined,
					$magazine = undefined,
					$counts = undefined;
				return{
					init: function () {
						$editor = $('#product_editor');
						$removeBtn = $('#remove_product');
						$magazine = $('#form_magazine');

						if(!$editor.length || !$removeBtn.length || !$magazine.length) return false;
						$response = $editor.find('.response');
						$counts = $('#form_counts');

						ProductEditor.loadData();
					},
					events: function () {

						$editor
							.unbind('submit.updateProduct')
							.bind('submit.updateProduct', function (e) {
								e.preventDefault();

								return false;
							});

						$removeBtn
							.unbind('click.removeProduct')
							.bind('click.removeProduct', function (e) {
								e.preventDefault();
								var
									id = $(this).attr('data-id'),
									res = confirm('Вы уверены, что хотите удалить товар?');
								if(res){
									var url_remove = Utils.secureGetPath($editor.attr('data-remove'));
									if(url_remove && url_remove.length)
										$.ajax({
											url: url_remove,
											method: 'DELETE',
											data: $.param({id: id}),
											processData: false,
											dataType: 'json',
											cache: false,
											beforeSend: function () {
												$response.html('');
											},
											success: function (res) {
												console.log(res);
												if(res.success){
													alert('Товар успешно удален.');
													window.location = '/';
												}else
													alert('Не удалось удалить товар, попробуйте еще раз.');
											},
											error: function (res) {

											}
										});
								}
								return false;
							});

						if($magazine.length) {
							$('#form_magazine_add')
								.unbind('click.add')
								.bind('click.add', function (e) {
									e.preventDefault();
									var $clone = $magazine.find('select').first().clone();

									$clone.find('option').each(function (idx, element) {
										if(idx)
											$(element).attr('selected', false);
										else
											$(element).attr('selected', true);
									});
									$(this).before($clone[0].outerHTML);
									console.log($counts);
									$clone = $counts.find('input').eq(0).clone();
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

									if (idx) {
										$elements.eq(idx).remove();
										$elements = $counts.find('input');
										$elements.eq(idx).remove();
									} else {
										$magazine.find('select').eq(0)
											.val('');
										$counts.find('input').eq(0)
											.val('');
									}

									return false;
								});
						}
					},
					loadData: function () {
						Utils.loadSelects($editor);

						var queryString = Utils.queryString();
						console.log(queryString);
						if(queryString.id && queryString.id.length){
							var
								url = $($editor).attr('data-load'),
								xhr = function () { return $.ajaxSettings.xhr(); };

							$.ajax({
								url: url,
								method: 'GET',
								data: $.param({id: queryString.id}),
								processData: false,
								dataType: 'json',
								cache: false,
								xhr: xhr,
								beforeSend: function () {
									$response.html('');
								},
								success: function (res) {
									console.log(res);
									if(res.success && res.item != false){
										var
											item = res.item,
											stores = [],
											counts = [];

										if(item.stores){
											stores = item.stores;
											delete item.stores;
										}
										if(item.counts){
											counts = item.counts;
											delete item.counts;
										}
										
										console.log(typeof counts);
										
										if(stores != [] && counts != []){
											Utils.createStoresData(
												$('#form_magazine'),
												$('#form_counts'),
												stores,
												counts
											);
										}

										Utils.fillDataToForm($editor, res.item);
										$removeBtn.attr('data-id', res.item.id);
										ProductEditor.events();
									}
								},
								error: function (res) {

								}
							});
						}
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
					$timepicker = undefined,

					$enableBtn = undefined,

					$magazine = undefined,
					$counts = undefined,

					$doubleInputs = undefined,
					$integerInputs = undefined,
					i = 0, j = 0, k = 0,
					length = 0;
				return{
					init: function () {
						$form = $('#send_form');
						if(!$form.length) return false;
						Form.loadUrl();

						$response = $form.find('.response');
						$progressBar = $form.find('.progress_bar');
						$timepicker = $form.find('.time_picker');

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
						var init_url = $form.attr('data-init');
						if(init_url && init_url.length){
							init_url = Utils.secureGetPath(init_url);
							if(init_url)
								Form.send($form, init_url);
						}

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
									if (/\D/g.test(this.value))
									{
										this.value = this.value.replace(/\D/g, '');
									}
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

						if($timepicker.length)
							$timepicker.datetimepicker({
								datepicker: false,
								format: 'H:i'
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
					send: function (that, init_url) {
						var
							url = init_url || $(that).attr('data-url'),
							method = $(that).attr('data-method'),
							view = $(that).attr('data-view'),
							moduleNames = $(that).attr('data-modules'),
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
							data: $form.serialize(),
							processData: false,
							dataType: 'json',
							cache: false,
							xhr: xhr,
							beforeSend: function () {
								$response.html('');
							},
							success: function (res) {
								if(res.success && res.template)
									$response.html($.parseHTML(res.template));

								var modules = [];
								if(moduleNames.length)
									modules = Utils.parseModuleNames(moduleNames);

								if(modules.indexOf('order') > -1)
									Order.init();
							},
							error: function (res) {
								if(res.status && res.statusText && res.statusText.length)
									$response.html(Alert.error('Ответ сервера: ' + res.statusText + ' [<b>' + res.status + '</b>]'));
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
			Order = (function () {
				var
					_items = [],

					$count = undefined,
					$orderBtn = undefined,

					i = 0, j = 0, k = 0,
					length = 0;
				return{
					init: function () {
						$count = $('#order_items_count');
						$orderBtn = $('.order_btn');

						Order.set();
						Order.events();
					},
					events: function () {
						if($orderBtn.length)
							$orderBtn.each(function (idx, element) {
								var id = parseInt($(element).attr('data-id'));
								if(!isNaN(id) && _items.length && _items.indexOf(id) > -1)
									Order.binder.from(element);
								else
									Order.binder.to(element);
							});
					},
					binder:{
						to: function (that) {
							$(that)
								.text('В заказ')
								.removeClass('from_order')
								.addClass('to_order')
								.unbind('click.addToOrder')
								.bind('click.addToOrder', function (e) {
									e.preventDefault();
									var id = parseInt($(this).attr('data-id'));

									if(id > 0)
										Order.add(id);

									Order.binder.from(that);

									return false;
								});
						},
						from: function (that) {
							$(that)
								.text('Отмена')
								.removeClass('to_order')
								.addClass('from_order')
								.unbind('click.addToOrder')
								.bind('click.addToOrder', function (e) {
									e.preventDefault();
									var id = parseInt($(that).attr('data-id'));

									if(id > 0)
										Order.remove(id);

									Order.binder.to(that);

									return false;
								});
						}
					},
					set: function (value) {
						if(!value) {
							var items = Storage.get('order');
							if(items && items.length) {
								_items = items;
							}
							else{
								_items = [];
								Order.save();
							}
						}else{
							_items = value;
							Order.save();
						}
					},
					get: function () {
						return _items;
					},
					add: function (value) {
						if(!isNaN(value) && typeof value === 'number') {
							if(_items.indexOf(value) == -1)
								_items.push(parseInt(value));
						}
						else{
							length = value.length;
							for(i = 0; i < length; i++)
								if(!isNaN(value) && _items.indexOf(value) == -1)
									_items.push(parseInt(value[i]));
						}

						Order.save();
					},
					remove: function (id) {
						var items = [];
						length = _items.length;

						for(i = 0; i < length; i++)
							if(_items[i] != id)
								items.push(_items[i]);

						Order.set(items);
					},
					save: function () {
						Storage.set('order', _items);
						length = _items.length;
						$count.text(length);
					},
					clear: function () {
						_items = [];
						Order.save();
					}
				}
			})(),
			Storage = (function () {
				return{
					set: function (key, value) {
						localStorage.setItem(key, JSON.stringify(value));
					},
					get: function (key) {
						return JSON.parse(localStorage.getItem(key));
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
							{name: 'Список', link: '/products/list'},
							{name: 'Создать', link: '/products/create'},
							{name: 'Редактировать', link: '/products/edit'}
						]},
						{name: 'Магазины', link: [
							{name: 'Список', link: '/stores.list.html'},
							{name: 'Создать', link: '/stores.create.html'},
							{name: 'Редактировать', link: '/stores.edit.html'},
							{name: 'Отчеты', link: '/stores.reviews.html'}
						]},
						{name: 'Производители', link: [
							{name: 'Список', link: '/stores.list.html'},
							{name: 'Создать', link: '/producers/create'},
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
