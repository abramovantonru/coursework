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

					$doubleInputs = undefined;
				return{
					init: function () {
						$form = $('#send_form');
						if(!$form.length) return false;

						$response = $form.find('.response');
						$progressBar = $form.find('.progress_bar');

						$doubleInputs = $form.find('.only_double');

						if(typeof $progressBar !== 'undefined')
							$progressBar.progressbar({
								max: 100,
								value: 0
							});


						this.events();
					},
					events: function () {
						$form
							.unbind('submit.sendData')
							.bind('submit.sendData', function (e) {
								e.preventDefault();
								Form.send(this);
								return false;
							});

						$doubleInputs
							.unbind('input.onlyDouble')
							.bind('input.onlyDouble', function (e) {
								e.preventDefault();
								this.value = this.value.replace(/[^0-9.]/g, '').replace(/(\..*)\./g, '$1');
								return false;
							});
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
					items = [
						{name: 'Покупки', link: '123'},
						{name: 'Товары', link: [
							{name: 'Список', link: '/products.list.html'},
							{name: 'Создать', link: '/products.create.html'},
							{name: 'Редактировать', link: '/products.edit.html'}
						]}
					],
					i = 0, j = 0,
					length = 0,
					html = '',
					submenu = [],
					$submenu = undefined,
					height = window.innerHeight;
				return{
					init: function () {
						length = items.length;
						$menu = $('#menu');
						if(!length || !$menu.length) return false;
						
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
							}else if(type === 'string' && item.link != ''){
								html += '<a href="' + item.link + '" class="menu-item">' + item.name + '</a>';
							}
						}

						$menu.html($.parseHTML(html));

						//$submenu = $('.submenu');

						this.events();
					},
					events: function () {
						$menu.resizable({
							minWidth: 100,
							animate: true,
							handles: 'e',
							animateDuration: 'fast',
							resize: function( event, ui ) {
								ui.size.height = height;
							}
						});


						$menu
							.unbind('click.toggleMenu')
							.bind('click.toggleMenu', function (e) {
								e.preventDefault();
								console.log(123);
								return false;
							});

						$('.menu-item')
							.unbind('click.selectItem')
							.bind('click.selectItem', function (e) {
								e.preventDefault();
								var url = $(this).attr('href');
								if(url == '#'){
									var idx = $('.menu-item').index(this);
									Menu.submenu.init(idx);
									//Menu.submenu.show(idx);
								}else
									window.location = url;

								return false;
							});
					},
					submenu:{
						init: function(idx){
							console.log(idx);
							console.log(submenu[idx]);
							console.log(submenu);
						},
						show: function (idx) {
							
						},
						hide: function (idx) {
							
						}
					},
					show: function () {
						$menu.addClass('open');
					},
					hide: function () {
						$menu.removeClass('open');
					}
				}
			})();

		App.init();
		console.log('jQuery ready');

	})();
});

console.timeEnd('main');
