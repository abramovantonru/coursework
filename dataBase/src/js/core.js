console.time('main');

$(document).ready(function () {
	(function () {

		var App = (function () {
				return{
					init: function () {
						Menu.init();
						Form.init();
					}
				}
			})(),
			Utils = (function () {
				return{

				}
			})(),
			Form = (function () {
				var
					$form = undefined,
					$response = undefined,
					$progressBar = undefined,

					$magazine = undefined,
					$counts = undefined,

					$doubleInputs = undefined,
					$integerInputs = undefined;
				return{
					init: function () {
						$form = $('#send_form');
						if(!$form.length) return false;

						$response = $form.find('.response');
						$progressBar = $form.find('.progress_bar');

						$magazine = $('#form_magazine');
						$counts = $('#form_counts');

						$doubleInputs = $form.find('.only_double');
						$integerInputs = $form.find('.only_integer');

						if(typeof $progressBar !== 'undefined')
							$progressBar.progressbar({
								max: 100,
								value: 0
							});

						this.events();
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

						if($magazine.length){
							$('#form_magazine_add')
								.unbind('click.add')
								.bind('click.add', function (e) {
									e.preventDefault();
									var $clone = $magazine.find('select').first().clone();
									
									$clone
										.attr('value', '')
										.attr('required', false);
									$(this).before($clone[0].outerHTML);

									$clone = $counts.find('input').first().clone();
									$clone
										.attr('value', '')
										.attr('required', false);
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
					validation: function ($form) {
						var error = '';

						$form.find('*').each(function (idx, element) {
							if($(element).attr('data-valid') === 'novalid') return true;
							
							var name = $('label[for="' + $(element).attr('id') + '"]').text();
							
							if($(element).attr('required') === 'required' && $(element).val() == '')
								error = 'Не заполнено обязательное поле "' + name + '"!';
						});
						if(error.length){
							console.log(error);
							return false;
						}

						return true;
					},
					send: function (that) {
						var
							url = $(that).attr('data-url'),
							method = $(that).attr('data-method'),
							data = new FormData(that[0]);
						$.ajax({
							url: url,
							method: method,
							processData: false,
							contentType: 'multipart/form-data',
							dataType: 'json',
							cache: false,
							xhr: function(){
								var xhr = $.ajaxSettings.xhr();
								xhr.upload.addEventListener('progress', function(e){
									if(e.lengthComputable) {
										var progress = Math.ceil(e.loaded / e.total * 100);
										$progressBar.progressbar('value', progress);
									}
								}, false);
								return xhr;
							},
							success: function (res) {
								console.log(res);
							},
							error: function (res) {
								console.log(res);
							}
						});
					}
				}
			})(),
			Menu = (function () {
				var
					$menu = undefined,
					$submenu = undefined,
					$menu_item = undefined,

					items = [
						{name: 'Покупки', link: '123'},
						{name: 'Товары', link: [
							{name: 'Список', link: '/products.list.html'},
							{name: 'Создать', link: '/products.create.html'},
							{name: 'Редактировать', link: '/products.edit.html'}
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

						this.events();
					},
					events: function () {
						$menu.resizable({
							minWidth: 100,
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
						console.log('toggle');
						if($menu.hasClass('open'))
							Menu.hide();
						else
							Menu.show();
					}
				}
			})();

		App.init();
		console.log('jQuery ready');

	})();
});

console.timeEnd('main');
