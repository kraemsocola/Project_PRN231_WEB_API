(function($) {

	'use strict';

	var windowsize = $(window).width();
	var $filterPageUrl = [];
	var $nextPageUrlShop = [];
	var $filterNextPageUrl = [];
    var $shop_default_container = [];
	var $shop_carousel_container = [];
	var $shop_masonry_container = [];
	var $shop_fullwidth_container = [];

	////*************************************** Shop Filter General Functions ********************************////

	function checkUrlParam(url, param) {
		var results = new RegExp('[\?&]' + param + '=([^&#]*)').exec(url);
		if (results == null) {
			return 0;
		} else {
			return results[1] || 0;
		}
	}

	function replaceUrlParam(url, paramName, paramValue) {
		if (paramValue == null) paramValue = '';
		var pattern = new RegExp('\\b(' + paramName + '=).*?(&|$)')
		if (url.search(pattern) >= 0) {
			return url.replace(pattern, '$1' + paramValue + '$2');
		}
		return url + (url.indexOf('?') > 0 ? '&' : '?') + paramName + '=' + paramValue;
	}

	function removeUrlParam(url, paramName) {
		var rtn = url.split("?")[0],
			param, params_arr = [],
			queryString = (url.indexOf("?") !== -1) ? url.split("?")[1] : "";
		if (queryString !== "") {
			params_arr = queryString.split("&");
			for (var i = params_arr.length - 1; i >= 0; i -= 1) {
				param = params_arr[i].split("=")[0];
				if (param === paramName) {
					params_arr.splice(i, 1);
				}
			}
			if (params_arr.length > 0) {
				rtn = rtn + "?" + params_arr.join("&");
			}
		}
		return rtn;
	}

	function changeUrl(url) {
		var page = document.title;
		if (typeof(history.pushState) != "undefined") {
			var obj = {
				Page: page,
				Url: url
			};
			history.pushState(obj, obj.Page, obj.Url);
		} else {
			alert("Browser does not support HTML5.");
		}
	}

	function seeseShopLoadImages(indexNum) {

		var $dload = indexNum.data('dload');
		var $sload = indexNum.data('sload');
		var $lazySrc = indexNum.data('ldurl');

		if ( ( typeof $dload !== 'undefined' ) && ( $dload === 'seese-dload-small' ) && ( typeof $sload !== 'undefined') ) {

			if ( windowsize < $sload ) {

				indexNum.find('li .seese-loop-thumb .seese-unveil-loader').hide();

				indexNum.children('li').each(function(index) {
					var $this = $(this).find('.seese-loop-thumb .wp-post-image');
					var $realSrc = $this.attr('data-src');
					$this.attr('src', $realSrc);
				});

			} else {

				$(window).off('scroll.unveil resize.unveil lookup.unveil');
				indexNum.find('li .seese-loop-thumb .seese-unveil-loader').show();

				indexNum.children('li').each(function(index) {
					var $this = $(this).find('.seese-loop-thumb .wp-post-image');
					$this.attr('src', $lazySrc);
				});

				var $imagesLoaded = indexNum.find('li:not(.seese-image-loaded) .seese-loop-thumb .seese-unveil-image');

			    if ($imagesLoaded.length) {
					var scrollTolerance = 1;
					$imagesLoaded.unveil(scrollTolerance, function() {
					    $(this).load(function() {
					        $(this).parents('li').first().addClass('seese-image-loaded');
					    });
					});
				}

				$(window).scrollTop($(window).scrollTop()+1);
			}

		} else {

			$(window).off('scroll.unveil resize.unveil lookup.unveil');
			var $imagesLoaded = indexNum.find('li:not(.seese-image-loaded) .seese-loop-thumb .seese-unveil-image');

			if ($imagesLoaded.length) {
				var scrollTolerance = 1;
				$imagesLoaded.unveil(scrollTolerance, function() {
				    $(this).load(function() {
				        $(this).parents('li').first().addClass('seese-image-loaded');
				    });
				});
			}

			$(window).scrollTop($(window).scrollTop()+1);
		}
	}

	////************************************* Mini Cart Widget Script Starts ********************************////

	$(window).load(function() {

		// Add Custom Scroll Cart Modal
		$("#seese-aside ul.cart_list").mCustomScrollbar({
			axis: "y"
		});

		// Remove Button Class From Cart Modal
		$("#seese-aside p.buttons a").removeClass("button");

	    // Open Cart Modal
	    $('#seese-cart-trigger').click(function(e) {
			e.preventDefault();
			document.getElementById("seese-aside").style.right = "0";
			document.body.style.overflowX = "hidden";
			document.getElementById("seese-closebtn").style.visibility = "visible";
			document.getElementById("seese-closebtn").style.opacity = "1";
		});

		// Close Cart Modal
		$('#seese-closebtn').click(function(e) {
			e.preventDefault();
			document.getElementById("seese-aside").style.right = "-360px";
			document.getElementById("seese-closebtn").style.visibility = "hidden";
			document.getElementById("seese-closebtn").style.opacity = "0";
		});

	});

	////*************************************** Document Ready Functions ********************************////

	$(document).ready(function() {

		seeseShopLoadImages($('ul.products'));

		// OnClick Focus Search Modal
		$("#seese-search-modal").on("shown.bs.modal", function() {
			$("#seese-prs").focus();
		})

		////****************************************** Product ShortCode Scripts Starts ***************************************////

		$(".seese-shop-wrapper").each(function(index) {
			$(this).attr('data-scnumber', index);
			$filterPageUrl[index] = $(location).attr('href');
			$nextPageUrlShop[index] = $(this).find('.seese-load-more-link a').attr('href');
			$filterNextPageUrl[index] = $(this).find('.seese-load-more-link a').attr('href');

			if ($(this).find('.seese-products-full-wrap').hasClass('seese-prsc-shop-masonry')) {
				$(this).find('.seese-products-full-wrap ul.products li').not(".seese-pr-masonry-sizer, .seese-pr-gutter-sizer").addClass('seese-pr-masonry-item');
			}

			if ($(this).find('.seese-products-full-wrap').hasClass('seese-pr-fullgrid')) {
				$(this).find('.seese-products-full-wrap ul.products li .seese-product-cnt').prepend(function() {
					return '<div class="seese-product-text top">' + $(this).find('.seese-product-text').html() + '</div>';
				});
			}

			if ($(this).find('.seese-filterWrap').hasClass('seese-filter-active')) {
				$(this).find('.seese-filterAll li a').addClass('seese-hide-filter');
				$(this).find('.seese-filterOptions #seese-filter').addClass('in');
		    }
		});

		$('.seese-shop-wrapper .seese-filterAll li a').click(function(event) {
			$(this).toggleClass('seese-hide-filter');
		});

		$('.seese-shop-wrapper .seese-filterOptions h3').click(function(event) {
			$(this).toggleClass('collapsable');
		});

		$(".seese-shop-wrapper .seese-prsc-shop-masonry ul.products").each(function(index) {
			$shop_masonry_container[index] = $(this);
			$shop_masonry_container[index].imagesLoaded(function() {
				$shop_masonry_container[index].isotope({
					itemSelector: '.seese-pr-masonry-item',
					masonry: {
						columnWidth: '.seese-pr-masonry-sizer',
						gutter: '.seese-pr-gutter-sizer',
					}
				});
			});
		});

		$(".seese-shop-wrapper .seese-prsc-shop-carousel .owl-carousel").each(function(index) {
			$shop_carousel_container[index] = $(this);
			$shop_carousel_container[index].owlCarousel({
				margin: 40,
				items: $shop_carousel_container[index].data("items"),
				loop: $shop_carousel_container[index].data("loop"),
				nav: $shop_carousel_container[index].data("nav"),
				dots: $shop_carousel_container[index].data("dots"),
				autoplay: $shop_carousel_container[index].data("autoplay"),
				navSpeed: $shop_carousel_container[index].attr('navspeed'),
				dotsSpeed: $shop_carousel_container[index].attr('dotspeed'),
				autoplaySpeed: $shop_carousel_container[index].attr('autospeed'),
				navText: ["<i class='fa fa-angle-left' aria-hidden='true'></i>", "<i class='fa fa-angle-right' aria-hidden='true'></i>"],
				responsive: {
					0: {
						items: 1
					},
					480: {
						items: 2
					},
					767: {
						items: 3
					},
					991: {
						items: $shop_carousel_container[index].data("items"),
					}
				}
			});
		});

    $(".seese-shop-wrapper .seese-prsc-shop-default ul.products").each(function(index) {
			$shop_default_container[index] = $(this);
			$shop_default_container[index].imagesLoaded(function() {
	    	$shop_default_container[index].children('li').matchHeight({
	        	byRow: false
	    	});
	    });
		});

		$(".seese-shop-wrapper .seese-prsc-shop-fullwidth ul.products").each(function(index) {
			$shop_fullwidth_container[index] = $(this);
		});

		////************************************* Shop All Filter Variable Declaration ********************************////

		var $shopPageUrl = $('.seese-shop-wrapper .seese-products-full-wrap').attr('data-shopurl');
		var $initSortBy = $('#seese-product-sorting li.active a').attr('data-sort');
		var $initMinPrice = $('#seese-price-filter .price_slider_amount #min_price').val();
		var $initMaxPrice = $('#seese-price-filter .price_slider_amount #max_price').val();

        if ( typeof woocommerce_price_slider_params === 'undefined' ) {
		    var $initMinHtml = '';
            var $initMaxHtml = '';
    	} else {
    	    if ( woocommerce_price_slider_params.currency_pos === 'left' ) {
                var $initMinHtml = woocommerce_price_slider_params.currency_symbol + $initMinPrice;
                var $initMaxHtml = woocommerce_price_slider_params.currency_symbol + $initMaxPrice;
            } else if ( woocommerce_price_slider_params.currency_pos === 'left_space' ) {
                var $initMinHtml = woocommerce_price_slider_params.currency_symbol + ' ' + $initMinPrice;
                var $initMaxHtml = woocommerce_price_slider_params.currency_symbol + ' ' + $initMaxPrice;
            } else if ( woocommerce_price_slider_params.currency_pos === 'right' ) {
                var $initMinHtml = min + woocommerce_price_slider_params.currency_symbol;
                var $initMaxHtml = max + woocommerce_price_slider_params.currency_symbol;
            } else if ( woocommerce_price_slider_params.currency_pos === 'right_space' ) {
                var $initMinHtml = min + ' ' + woocommerce_price_slider_params.currency_symbol;
                var $initMaxHtml = max + ' ' + woocommerce_price_slider_params.currency_symbol;
            }
    	}

		////************************************* Shop Ajax Filter Scripts Starts ********************************////

		function doProductAjaxFilter(urlFilter, getScIndex) {

			var proCat = decodeURIComponent(checkUrlParam(urlFilter, 'product_cat'));
			var orderBy = decodeURIComponent(checkUrlParam(urlFilter, 'orderby'));
			var minPrice = decodeURIComponent(checkUrlParam(urlFilter, 'min_price'));
			var maxPrice = decodeURIComponent(checkUrlParam(urlFilter, 'max_price'));

			if (orderBy != 0) {
				if (orderBy == $initSortBy) {
					$filterPageUrl[getScIndex] = removeUrlParam($filterPageUrl[getScIndex], 'orderby');
				}
			}

			if ((minPrice != 0) && (maxPrice != 0)) {
				if ((minPrice == $initMinPrice) && (maxPrice == $initMaxPrice)) {
					var minChangedUrl = removeUrlParam($filterPageUrl[getScIndex], 'min_price');
					$filterPageUrl[getScIndex] = removeUrlParam(minChangedUrl, 'max_price');
				}
			}

			if (proCat != 0) {
				if (proCat == 'all') {
					$filterPageUrl[getScIndex] = removeUrlParam($filterPageUrl[getScIndex], 'product_cat');
				}
			}

			var queryStringPart = ($filterPageUrl[getScIndex].indexOf("?") !== -1) ? $filterPageUrl[getScIndex].split("?")[1] : 0;
			var $finalFilterPageUrl;

			if (queryStringPart != 0) {
				$finalFilterPageUrl = $shopPageUrl + "?" + queryStringPart;
			} else {
				$finalFilterPageUrl = $filterPageUrl[getScIndex];
			}

			if ($(".seese-shop-wrapper[data-scnumber='" + getScIndex + "']").hasClass('seese-no-ajax')) {

				window.location.href = $finalFilterPageUrl;

			} else if ($(".seese-shop-wrapper[data-scnumber='" + getScIndex + "']").hasClass('seese-ajax')) {

				//changeUrl($filterPageUrl[getScIndex]);

				if ($finalFilterPageUrl) {

					if ($(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-shop-load-more-controls").hasClass('seese-all-loaded')) {
						$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-shop-load-more-controls").removeClass('seese-all-loaded');
					}

					if ($(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-shop-load-more-controls").hasClass('seese-hide-btn')) {
						$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-shop-load-more-controls").removeClass('seese-hide-btn');
					}

					$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "']").find('.seese-shop-load-more-controls #seese-loaded').addClass('seese-link-present');
					$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "']").find('.seese-shop-load-anim').addClass('seese-loader');
					$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "']").find('.seese-products-full-wrap').fadeOut(500);
					$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "']").find('.seese-shop-load-more-box').fadeOut(500);

					$.ajax({
						method: 'POST',
						url: $finalFilterPageUrl,
						dataType: 'html',
						error: function(XMLHttpRequest, textStatus, errorThrown) {
							console.log('SEESE: AJAX error - ' + errorThrown);
						},
						complete: function() {
							$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "']").find('.seese-shop-load-anim').removeClass('seese-loader');
							$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "']").find('.seese-products-full-wrap').fadeIn(500);
							$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "']").find('.seese-shop-load-more-box').fadeIn(500);
						},
						success: function(response) {
							var $filteredProducts = '';
							var $filteredProductsCarousel = '';
							var $filteredProductsMasonry = '';
							var $checkShopNumber = 0;

							$(".seese-shop-wrapper", $.parseHTML(response)).each(function(index) {
								$checkShopNumber = $checkShopNumber + index;
							});

							if ($checkShopNumber == 0) {
								$filteredProducts = $($.parseHTML(response)).find('.seese-shop-wrapper .seese-products-full-wrap ul.products').children('li.type-product');
								$filteredProductsMasonry = $($.parseHTML(response)).find('.seese-shop-wrapper .seese-products-full-wrap ul.products').children('li.type-product').addClass('seese-pr-masonry-item');

							} else {
								$(".seese-shop-wrapper", $.parseHTML(response)).each(function(index) {
									var $this = $(this);
									if (parseInt(index) == parseInt(getScIndex)) {
										$filteredProducts = $this.find('.seese-products-full-wrap ul.products').children('li.type-product');
										$filteredProductsMasonry = $this.find('.seese-products-full-wrap ul.products').children('li.type-product').addClass('seese-pr-masonry-item');
									}
								});
							}

							if ($filteredProducts.length) {

								if ($(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-products-full-wrap").hasClass('seese-prsc-shop-masonry')) {

									var $deleteProductsMasonry = $(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-products-full-wrap ul.products").children('li.type-product');
									$shop_masonry_container[getScIndex].isotope('remove', $deleteProductsMasonry).isotope('layout');

									$.each($filteredProductsMasonry, function( key, el ) {
										var seeseProductText  = $(el).find('.seese-product-text').html();
										$(el).find('.seese-product-cnt').prepend('<div class="seese-product-text top">' + seeseProductText + '</div>');
										if ($(el).hasClass('pd-2wh')) {
											$(el).find('.seese-loop-thumb img.wp-post-image').removeAttr('width');
											$(el).find('.seese-loop-thumb img.wp-post-image').removeAttr('height');
											var seeseProductMainImage = $(el).find('.seese-loop-thumb img.wp-post-image').attr('data-src-main');
											$(el).find('.seese-loop-thumb img.wp-post-image').attr('src', seeseProductMainImage);
											$(el).find('.seese-loop-thumb img.wp-post-image').attr('data-src', seeseProductMainImage);
										}
										if ($(el).find('.seese-loop-thumb').hasClass('seese-loop-thumb-has-hover')) {
											$(el).find('.seese-loop-thumb .seese-hover-image-wrap img.seese-pr-hover-image').removeAttr('width');
											$(el).find('.seese-loop-thumb .seese-hover-image-wrap img.seese-pr-hover-image').removeAttr('height');
											var seeseProductHoverImage = $(el).find('.seese-loop-thumb .seese-hover-image-wrap img.seese-pr-hover-image').attr('data-src-main');
											$(el).find('.seese-loop-thumb .seese-hover-image-wrap img.seese-pr-hover-image').attr('src', seeseProductHoverImage);
											$(el).find('.seese-loop-thumb .seese-hover-image-wrap img.seese-pr-hover-image').attr('data-src', seeseProductHoverImage);
										}
									});

									$shop_masonry_container[getScIndex].append($filteredProductsMasonry).imagesLoaded(function() {
										$shop_masonry_container[getScIndex].isotope('appended', $filteredProductsMasonry).isotope('layout');
									});

									seeseShopLoadImages($shop_masonry_container[getScIndex]);

								} else if ($(".seese-shop-wrapper[data-scnumber='" + getScIndex + "']").find('.seese-products-full-wrap').hasClass('seese-prsc-shop-carousel')) {

									$shop_carousel_container[getScIndex].trigger('destroy.owl.carousel');
									$shop_carousel_container[getScIndex].html($shop_carousel_container[getScIndex].find('.owl-stage-outer').html()).removeClass('owl-loaded');

									$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-products-full-wrap").find('ul.products').empty();
									$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-products-full-wrap").find("ul.products").append($filteredProducts);

									$shop_carousel_container[getScIndex].owlCarousel({
										margin: 40,
										items: $shop_carousel_container[getScIndex].data("items"),
										loop: $shop_carousel_container[getScIndex].data("loop"),
										nav: $shop_carousel_container[getScIndex].data("nav"),
										dots: $shop_carousel_container[getScIndex].data("dots"),
										autoplay: $shop_carousel_container[getScIndex].data("autoplay"),
										navSpeed: $shop_carousel_container[getScIndex].attr('navspeed'),
										dotsSpeed: $shop_carousel_container[getScIndex].attr('dotspeed'),
										autoplaySpeed: $shop_carousel_container[getScIndex].attr('autospeed'),
										navText: ["<i class='fa fa-angle-left' aria-hidden='true'></i>", "<i class='fa fa-angle-right' aria-hidden='true'></i>"],
										responsive: {
									      	0: {
												items: 1
											},
											480: {
												items: 2
											},
											767: {
												items: 3
											},
											991: {
												items: $shop_carousel_container[getScIndex].data("items"),
											}
										}
									}).trigger('refresh.owl.carousel');

									seeseShopLoadImages($shop_carousel_container[getScIndex]);

                                } else if ($(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-products-full-wrap").hasClass('seese-prsc-shop-fullwidth')) {

                                    $.each($filteredProducts, function( key, el ) {
										var seeseProductText = $(el).find('.seese-product-text').html();
										$(el).find('.seese-product-cnt').prepend('<div class="seese-product-text top">' + seeseProductText + '</div>');
									});

                                   	$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-products-full-wrap").find("ul.products").empty();
									$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-products-full-wrap").find("ul.products").append($filteredProducts);

									seeseShopLoadImages($(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-products-full-wrap").find('ul.products'));

								} else if ($(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-products-full-wrap").hasClass('seese-prsc-shop-default')) {

									$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-products-full-wrap").find("ul.products").empty();
									$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-products-full-wrap").find("ul.products").append($filteredProducts);

                                    $shop_default_container[getScIndex].children('li').matchHeight({
                                        byRow: false
                                    });

                                    seeseShopLoadImages($shop_default_container[getScIndex]);

								} else {

									$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-products-full-wrap").find("ul.products").empty();
									$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-products-full-wrap").find("ul.products").append($filteredProducts);

                                    seeseShopLoadImages($(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-products-full-wrap").find("ul.products"));

								}

								$filterNextPageUrl[getScIndex] = $($.parseHTML(response)).find('.seese-shop-load-more-link').children('a').attr('href');

								if ($filterNextPageUrl[getScIndex]) {
									$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-load-more-link").find('a').attr('href', $filterNextPageUrl[getScIndex]);
									$nextPageUrlShop[getScIndex] = $(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-load-more-link").find('a').attr('href');
								} else {
									$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-shop-load-more-controls").addClass('seese-all-loaded');
									$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-load-more-link").find('a').removeAttr('href');
									$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "']").find('.seese-shop-load-more-controls #seese-loaded').removeClass('seese-link-present');
								}
							} else {
								$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-products-full-wrap").find('ul.products').empty();
								$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-shop-load-more-controls").addClass('seese-hide-btn');
								$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-products-full-wrap").find('ul.products').html('No Data for this Combinaion.');
							}
						}
					});
				}
			}
		}

		////************************************* Shop Sort Filter Scripts Starts ********************************////

		$('.seese-product-sorting li a').click(function(e) {
			e.preventDefault();
			var $this = $(this);
			var $thisLi = $this.parent('li');
			if ($thisLi.hasClass('active')) {
				return;
			}
			$thisLi.parent('ul').children('.active').removeClass('active');
			$thisLi.addClass('active');

			var sortData = $this.attr('data-sort');
			var getScIndex = $thisLi.parent('ul').closest(".seese-shop-wrapper").attr('data-scnumber');

			if (getScIndex == undefined) {
				getScIndex = $(".seese-shop-wrapper").attr('data-scnumber');
			}

			var orderBy = decodeURIComponent(checkUrlParam($filterPageUrl[getScIndex], 'orderby'));

			if (orderBy != 0) {
				$filterPageUrl[getScIndex] = replaceUrlParam($filterPageUrl[getScIndex], 'orderby', sortData);
			} else {
				if ($filterPageUrl[getScIndex].indexOf('?') > 0) {
					$filterPageUrl[getScIndex] += '&';
				} else {
					$filterPageUrl[getScIndex] += '?';
				}
				$filterPageUrl[getScIndex] += 'orderby=' + sortData;
			}

			doProductAjaxFilter($filterPageUrl[getScIndex], getScIndex);
		});

		////************************************* Shop Price Filter Scripts Starts ********************************////

		$('#seese-price-filter .price_slider_amount #seese-price-filter-submit').click(function(e) {
			e.preventDefault();
			var $this = $(this);
			var getScIndex = $this.parent('.price_slider_amount').closest(".seese-shop-wrapper").attr('data-scnumber');

			if (getScIndex == undefined) {
				getScIndex = $(".seese-shop-wrapper").attr('data-scnumber');
			}

			var minPrice = decodeURIComponent(checkUrlParam($filterPageUrl[getScIndex], 'min_price'));
			var maxPrice = decodeURIComponent(checkUrlParam($filterPageUrl[getScIndex], 'max_price'));
			var minValue = $this.parent('.price_slider_amount').find('#min_price').val();
			var maxValue = $this.parent('.price_slider_amount').find('#max_price').val();

			if ((minPrice == 0) && (maxPrice == 0)) {
				if ($filterPageUrl[getScIndex].indexOf('?') > 0) {
					$filterPageUrl[getScIndex] += '&';
				} else {
					$filterPageUrl[getScIndex] += '?';
				}
				$filterPageUrl[getScIndex] += 'min_price=' + minValue + '&max_price=' + maxValue;
			} else {
				var minChangedUrl = replaceUrlParam($filterPageUrl[getScIndex], 'min_price', minValue);
				$filterPageUrl[getScIndex] = replaceUrlParam(minChangedUrl, 'max_price', maxValue);
			}

			doProductAjaxFilter($filterPageUrl[getScIndex], getScIndex);
		});

		////*************************************** Shop Color Filter Scripts Starts ********************************////

		$('.seese-color-filter li a').click(function(e) {
			e.preventDefault();
			var $this = $(this);
			var $thisLi = $this.parent('li');
			var colorData = $this.attr('data-color');
			var attrnameData = $this.attr('data-attrname');
			var filterColor = 'filter_' + attrnameData;
			var getScIndex = $thisLi.parent('ul').closest(".seese-shop-wrapper").attr('data-scnumber');

			if (getScIndex == undefined) {
				getScIndex = $(".seese-shop-wrapper").attr('data-scnumber');
			}

			var filterName = decodeURIComponent(checkUrlParam($filterPageUrl[getScIndex], filterColor));

			if ($thisLi.hasClass('active')) {
				if (filterName != 0) {
					$filterPageUrl[getScIndex] = removeUrlParam($filterPageUrl[getScIndex], filterColor);
				}
				$thisLi.parent('ul').children('.active').removeClass('active');
			} else {
				$thisLi.parent('ul').children('.active').removeClass('active');
				$thisLi.addClass('active');

				if (filterName != 0) {
					$filterPageUrl[getScIndex] = replaceUrlParam($filterPageUrl[getScIndex], filterColor, colorData);
				} else {
					if ($filterPageUrl[getScIndex].indexOf('?') > 0) {
						$filterPageUrl[getScIndex] += '&';
					} else {
						$filterPageUrl[getScIndex] += '?';
					}
					$filterPageUrl[getScIndex] += filterColor + '=' + colorData;
				}
			}

			doProductAjaxFilter($filterPageUrl[getScIndex], getScIndex);
		});

		////*************************************** Shop Brand Filter Scripts Starts ********************************////

		$('.seese-brand-filter li a').click(function(e) {
			e.preventDefault();
			var $this = $(this);
			var $thisLi = $this.parent('li');
			var brandData = $this.attr('data-brand');
			var attrnameData = $this.attr('data-attrname');
			var filterBrand = 'filter_' + attrnameData;
			var getScIndex = $thisLi.parent('ul').closest(".seese-shop-wrapper").attr('data-scnumber');

			if (getScIndex == undefined) {
				getScIndex = $(".seese-shop-wrapper").attr('data-scnumber');
			}

			var filterName = decodeURIComponent(checkUrlParam($filterPageUrl[getScIndex], filterBrand));

			if ($thisLi.hasClass('active')) {
				if (filterName != 0) {
					$filterPageUrl[getScIndex] = removeUrlParam($filterPageUrl[getScIndex], filterBrand);
				}
				$thisLi.parent('ul').children('.active').removeClass('active');
			} else {
				$thisLi.parent('ul').children('.active').removeClass('active');
				$thisLi.addClass('active');

				if (filterName != 0) {
					$filterPageUrl[getScIndex] = replaceUrlParam($filterPageUrl[getScIndex], filterBrand, brandData);
				} else {
					if ($filterPageUrl[getScIndex].indexOf('?') > 0) {
						$filterPageUrl[getScIndex] += '&';
					} else {
						$filterPageUrl[getScIndex] += '?';
					}
					$filterPageUrl[getScIndex] += filterBrand + '=' + brandData;
				}
			}

			doProductAjaxFilter($filterPageUrl[getScIndex], getScIndex);
		});

		////*************************************** Shop Attribute Filter Scripts Starts ********************************////

		$('.seese-attribute-filter li a').click(function(e) {
			e.preventDefault();
			var $this = $(this);
			var $thisLi = $this.parent('li');
			var attributeData = $this.attr('data-attribute');
			var attrnameData = $this.attr('data-attrname');
			var filterAttribute = 'filter_' + attrnameData;
			var getScIndex = $thisLi.parent('ul').closest(".seese-shop-wrapper").attr('data-scnumber');

			if (getScIndex == undefined) {
				getScIndex = $(".seese-shop-wrapper").attr('data-scnumber');
			}

			var filterName = decodeURIComponent(checkUrlParam($filterPageUrl[getScIndex], filterAttribute));

			if ($thisLi.hasClass('active')) {
				if (filterName != 0) {
					$filterPageUrl[getScIndex] = removeUrlParam($filterPageUrl[getScIndex], filterAttribute);
				}
				$thisLi.parent('ul').children('.active').removeClass('active');
			} else {
				$thisLi.parent('ul').children('.active').removeClass('active');
				$thisLi.addClass('active');

				if (filterName != 0) {
					$filterPageUrl[getScIndex] = replaceUrlParam($filterPageUrl[getScIndex], filterAttribute, attributeData);
				} else {
					if ($filterPageUrl[getScIndex].indexOf('?') > 0) {
						$filterPageUrl[getScIndex] += '&';
					} else {
						$filterPageUrl[getScIndex] += '?';
					}
					$filterPageUrl[getScIndex] += filterAttribute + '=' + attributeData;
				}
			}

			doProductAjaxFilter($filterPageUrl[getScIndex], getScIndex);
		});

		////*************************************** Shop Category Filter Scripts Starts ********************************////

		$('.seese-filterCategory li.seese-cat-item-all').addClass('active');

		$('.seese-filterCategory li a').click(function(e) {
			e.preventDefault();
			var $this = $(this);
			var $thisLi = $this.parent('li');
			if ($thisLi.hasClass('active')) {
				return;
			}
			$thisLi.parent('ul').children('.active').removeClass('active');
			$thisLi.addClass('active');

			var getScIndex = $thisLi.parent('ul').closest(".seese-shop-wrapper").attr('data-scnumber');

			if (getScIndex == undefined) {
				getScIndex = $(".seese-shop-wrapper").attr('data-scnumber');
			}

			if ($(".seese-shop-wrapper[data-scnumber='" + getScIndex + "']").has('.seese-product-sorting').length > 0) {
				$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "']").find('.seese-product-sorting').children('.active').removeClass('active');
				$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "']").find('.seese-product-sorting li.seese-sort-item-all').addClass('active');
			}

			if ($(".seese-shop-wrapper[data-scnumber='" + getScIndex + "']").has('.seese-color-filter').length > 0) {
				$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "']").find('.seese-color-filter').children('.active').removeClass('active');
			}

			if ($(".seese-shop-wrapper[data-scnumber='" + getScIndex + "']").has('.seese-brand-filter').length > 0) {
				$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "']").find('.seese-brand-filter').children('.active').removeClass('active');
			}

			if ($(".seese-shop-wrapper[data-scnumber='" + getScIndex + "']").has('.seese-attribute-filter').length > 0) {
				$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "']").find('.seese-attribute-filter').children('.active').removeClass('active');
			}

			if ($(".seese-shop-wrapper[data-scnumber='" + getScIndex + "']").has('#seese-price-filter').length > 0) {
				$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "']").find('#seese-price-filter .price_slider').slider("values", 0, $initMinPrice);
				$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "']").find('#seese-price-filter .price_slider').slider("values", 1, $initMaxPrice);
                $(".seese-shop-wrapper[data-scnumber='" + getScIndex + "']").find('#seese-price-filter .price_slider_amount span.from').html($initMinHtml);
			    $(".seese-shop-wrapper[data-scnumber='" + getScIndex + "']").find('#seese-price-filter .price_slider_amount span.to').html($initMaxHtml);
			}

			var curUrl = ($filterPageUrl[getScIndex].indexOf("?") !== -1) ? $filterPageUrl[getScIndex].split("?")[0] : $filterPageUrl[getScIndex];
			$filterPageUrl[getScIndex] = curUrl;
			var catData = $this.attr('data-procat');
			var filterCat = decodeURIComponent(checkUrlParam($filterPageUrl[getScIndex], 'product_cat'));

			if (filterCat != 0) {
				$filterPageUrl[getScIndex] = replaceUrlParam($filterPageUrl[getScIndex], 'product_cat', catData);
			} else {
				if ($filterPageUrl[getScIndex].indexOf('?') > 0) {
					$filterPageUrl[getScIndex] += '&';
				} else {
					$filterPageUrl[getScIndex] += '?';
				}
				$filterPageUrl[getScIndex] += 'product_cat=' + catData;
			}

			doProductAjaxFilter($filterPageUrl[getScIndex], getScIndex);
		});

		////************************************* Shop Load More Pagination Scripts Starts ********************************////

		$.each($nextPageUrlShop, function(getScIndex, value) {
			if ($nextPageUrlShop[getScIndex]) {
				$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-shop-load-more-controls #seese-loaded").addClass('seese-link-present');
			} else {
				$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-shop-load-more-controls").addClass('seese-hide-btn');
			}
		});

		$('.seese-shop-wrapper #seese-shop-load-more-btn').on('click', function(e) {

			e.preventDefault();

			var $this = $(this);
			var getScIndex = $this.parent('.seese-load-more-controls').closest(".seese-shop-wrapper").attr('data-scnumber');

			if ($nextPageUrlShop[getScIndex]) {

				if ($(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-shop-load-more-controls").hasClass('seese-hide-btn')) {
					$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-shop-load-more-controls").removeClass('seese-hide-btn');
				}

				$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-shop-load-more-controls").addClass('seese-loader');

				$.ajax({
					url: $nextPageUrlShop[getScIndex],
					dataType: 'html',
					method: 'GET',
					error: function(XMLHttpRequest, textStatus, errorThrown) {
						console.log('SEESE: AJAX error - ' + errorThrown);
					},
					complete: function() {
						$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-shop-load-more-controls").removeClass('seese-loader');
					},
					success: function(response) {
						var $newElements = '';
						var $newElementsMasonry = '';
						var $checkShopNumber = 0;

						$(".seese-shop-wrapper", $.parseHTML(response)).each(function(index) {
							$checkShopNumber = $checkShopNumber + index;
						});

						if ($checkShopNumber == 0) {
							$newElements = $($.parseHTML(response)).find('.seese-shop-wrapper .seese-products-full-wrap ul.products').children('li.type-product');
							$newElementsMasonry = $($.parseHTML(response)).find('.seese-shop-wrapper .seese-products-full-wrap ul.products').children('li.type-product').addClass('seese-pr-masonry-item');
						} else {
							$(".seese-shop-wrapper", $.parseHTML(response)).each(function(index) {
								var $this = $(this);
								if (parseInt(index) == parseInt(getScIndex)) {
									$newElements = $this.find('.seese-products-full-wrap ul.products').children('li.type-product');
									$newElementsMasonry = $this.find('.seese-products-full-wrap ul.products').children('li.type-product').addClass('seese-pr-masonry-item');
								}
							});
						}

						if ($(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-products-full-wrap").hasClass('seese-prsc-shop-masonry')) {

							$.each( $newElementsMasonry, function( key, el ) {
								var seeseProductText = $(el).find('.seese-product-text').html();
								$(el).find('.seese-product-cnt').prepend('<div class="seese-product-text top">' + seeseProductText + '</div>');
								if ($(el).hasClass('pd-2wh')) {
									$(el).find('.seese-loop-thumb img.wp-post-image').removeAttr('width');
									$(el).find('.seese-loop-thumb img.wp-post-image').removeAttr('height');
									var seeseProductMainImage = $(el).find('.seese-loop-thumb img.wp-post-image').attr('data-src-main');
									$(el).find('.seese-loop-thumb img.wp-post-image').attr('src', seeseProductMainImage);
									$(el).find('.seese-loop-thumb img.wp-post-image').attr('data-src', seeseProductMainImage);
								}
								if ($(el).find('.seese-loop-thumb').hasClass('seese-loop-thumb-has-hover')) {
									$(el).find('.seese-loop-thumb .seese-hover-image-wrap img.seese-pr-hover-image').removeAttr('width');
									$(el).find('.seese-loop-thumb .seese-hover-image-wrap img.seese-pr-hover-image').removeAttr('height');
									var seeseProductHoverImage = $(el).find('.seese-loop-thumb .seese-hover-image-wrap img.seese-pr-hover-image').attr('data-src-main');
									$(el).find('.seese-loop-thumb .seese-hover-image-wrap img.seese-pr-hover-image').attr('src', seeseProductHoverImage);
									$(el).find('.seese-loop-thumb .seese-hover-image-wrap img.seese-pr-hover-image').attr('data-src', seeseProductHoverImage);
								}
							});

                            $shop_masonry_container[getScIndex].append($newElementsMasonry).imagesLoaded(function() {
								$shop_masonry_container[getScIndex].isotope('appended', $newElementsMasonry).isotope('layout');
							});

							seeseShopLoadImages($shop_masonry_container[getScIndex]);

						} else if ($(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-products-full-wrap").hasClass('seese-prsc-shop-fullwidth')) {

							$.each($newElements, function( key, el ) {
								var seeseProductText = $(el).find('.seese-product-text').html();
								$(el).find('.seese-product-cnt').prepend('<div class="seese-product-text top">' + seeseProductText + '</div>');
							});

							$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-products-full-wrap").find('ul.products').append($newElements);

							seeseShopLoadImages($(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-products-full-wrap").find('ul.products'));

					    } else if ($(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-products-full-wrap").hasClass('seese-prsc-shop-default')) {

                            $(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-products-full-wrap").find('ul.products').append($newElements);

                            $shop_default_container[getScIndex].children('li').matchHeight({
                                remove: true
                            });

                            $shop_default_container[getScIndex].children('li').matchHeight({
                                byRow: false
                            });

                            seeseShopLoadImages($shop_default_container[getScIndex]);

						} else {
							$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-products-full-wrap").find('ul.products').append($newElements);
							seeseShopLoadImages($(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-products-full-wrap").find('ul.products'));
						}

						$nextPageUrlShop[getScIndex] = $($.parseHTML(response)).find('.seese-load-more-link').children('a').attr('href');

						if ($nextPageUrlShop[getScIndex]) {
							$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-load-more-link").find('a').attr('href', $nextPageUrlShop[getScIndex]);
						} else {
							$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-shop-load-more-controls").addClass('seese-all-loaded');
							$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-load-more-link").find('a').removeAttr('href');
							$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-shop-load-more-controls #seese-loaded").removeClass('seese-link-present');
						}
					}
				});
			} else {
				$(".seese-shop-wrapper[data-scnumber='" + getScIndex + "'] .seese-shop-load-more-controls").addClass('seese-hide-btn');
			}
		});

		////************************************* Product Single Page Script Starts ********************************////

		var $productImageSlider = $('#seese-product-images-slider');
		var $productImagesColumn = $('#seese-product-images-col');
		var $productImages = $productImageSlider.children('div');
		var $productThumbSlider = $('#seese-product-thumbnails-slider');
		var $productThumbs = $productThumbSlider.children('div');
		var $activeThumb = $productThumbs.first();
		var numThumbs = $productThumbs.length;
		var animSpeed = 300;
		var isThumbClick = false;
		var modalEnabled = $productImagesColumn.hasClass('modal-enabled');
		var zoomEnabled = $productImagesColumn.hasClass('zoom-enabled');
		var $relatedProCarousel = $('.seese-related-product-slider');

		$productImageSlider.on('beforeChange', function(event, slick, currentSlide, nextSlide) {
			if (!isThumbClick) {
				$productThumbSlider.find('.slick-slide').eq(nextSlide).trigger('click');
			}
			isThumbClick = false;
			$productImageSlider.addClass('animating');
		});

		$productImageSlider.on('afterChange', function() {
			$productImageSlider.removeClass('animating');
		});

		$productImageSlider.slick({
			adaptiveHeight: false,
			slidesToShow: 1,
			slidesToScroll: 1,
			prevArrow: '<a class="slick-large-img-prev"><i class="fa fa-angle-left"></i></a>',
			nextArrow: '<a class="slick-large-img-next"><i class="fa fa-angle-right"></i></a>',
			arrows: false,
			dots: false,
			fade: true,
			cssEase: 'linear',
			infinite: false,
			speed: animSpeed
		});

		$productThumbSlider.on('init', function() {
			$productThumbs.bind('click', function() {
				var $this = $(this);
				if ($productImageSlider.hasClass('animating') || $this.hasClass('current')) {
					return;
				}

				isThumbClick = true;
				$activeThumb.removeClass('current');
				$this.addClass('current');
				$activeThumb = $this;

				if (!$this.next().hasClass('slick-active')) {
					$productThumbSlider.slick('slickNext');
				} else if (!$this.prev().hasClass('slick-active')) {
					$productThumbSlider.slick('slickPrev');
				}

				$productImageSlider.slick('slickGoTo', $this.index(), false);
			});
		});

		
		$(window).on('load resize', function(e){
			if ( $(window).width() < 768 ) {
				$productThumbSlider.slick({
					slidesToShow: 3,
					slidesToScroll: 1,
					arrows: false,
					prevArrow: '<a class="slick-thumb-img-prev"><i class="fa fa-angle-left"></i></a>',
					nextArrow: '<a class="slick-thumb-img-next"><i class="fa fa-angle-right"></i></a>',
					infinite: false,
					vertical: false,
					focusOnSelect: false,
					draggable: false,
					speed: animSpeed,
					swipe: false,
					touchMove: false
				});
			} else{
				$productThumbSlider.slick({
					slidesToShow: 4,
					slidesToScroll: 1,
					arrows: false,
					prevArrow: '<a class="slick-thumb-img-prev"><i class="fa fa-angle-left"></i></a>',
					nextArrow: '<a class="slick-thumb-img-next"><i class="fa fa-angle-right"></i></a>',
					infinite: false,
					vertical: true,
					focusOnSelect: false,
					draggable: false,
					speed: animSpeed,
					swipe: false,
					touchMove: false
				});
		    }

		    // console.log(isVertical + ' + ' + slidesToShow);
		});




		$(window).on('resize',function(e){	
			setTimeout(function(){
				$productThumbSlider.slick('refresh');

				console.log(' refresh done! ');
			}, 500);
		});

		

		// Tab Activation
		$('.wc-tabs-wrapper ul.wc-tabs li').removeClass('active');
		$('.wc-tabs-wrapper .wc-tab').removeClass('active');

		$('.wc-tabs-wrapper ul.wc-tabs li.description_tab').addClass('active');
		$('.wc-tabs-wrapper #tab-description').addClass('active');

		$('.wc-tabs-wrapper ul.wc-tabs li a').click(function(e) {
			var $this = $(this);
			var $thisHref = $this.attr('href');

			if ($($thisHref).hasClass('active')) {
				return;
			}

			$('.wc-tabs-wrapper .woocommerce-Tabs-panel').removeClass('active');
			$($thisHref).addClass('active');
		});

		// Related Product Carousel
		$relatedProCarousel.owlCarousel({
		    loop: $relatedProCarousel.data("loop"),
            margin: 40,
            mouseDrag: false,
            touchDrag: false,
            items: $relatedProCarousel.data("items"),
			nav: $relatedProCarousel.data("nav"),
			dots: $relatedProCarousel.data("dots"),
			autoplay: $relatedProCarousel.data("autoplay"),
			navText: ["<i class='fa fa-angle-left' aria-hidden='true'></i>", "<i class='fa fa-angle-right' aria-hidden='true'></i>"],
			responsive: {
				0: {
					items: 1
				},
				480: {
					items: 2
				},
				767: {
					items: 3
				},
				991: {
					items: $relatedProCarousel.data("items")
				}
			}
		});

		// Gallery Update image (in case a variation image is used)
		var $variationsForm = $('.variations_form');
		var $variationsWrap = $variationsForm.children('.variations');

		$.fn.wc_set_variation_attr = function( attr, value ) {
			if ( undefined === this.attr( 'data-o_' + attr ) ) {
				this.attr( 'data-o_' + attr, ( ! this.attr( attr ) ) ? '' : this.attr( attr ) );
			}
			if ( false === value ) {
				this.removeAttr( attr );
			} else {
				this.attr( attr, value );
			}
		};

		$.fn.wc_reset_variation_attr = function( attr ) {
			if ( undefined !== this.attr( 'data-o_' + attr ) ) {
				this.attr( attr, this.attr( 'data-o_' + attr ) );
			}
		};

		function resetVaritaion() {
			var $product      = $variationsForm.closest( '.product' ),
			$product_gallery  = $product.find( '#seese-product-thumbnails-slider' ),
			$product_img_wrap = $product_gallery.find( '.woocommerce-product-gallery__image' ).eq( 0 ),
			$product_img      = $product_img_wrap.find( '.wp-post-image' );

			$product_img.wc_reset_variation_attr( 'src' );
			$product_img.wc_reset_variation_attr( 'width' );
			$product_img.wc_reset_variation_attr( 'height' );
			$product_img.wc_reset_variation_attr( 'srcset' );
			$product_img.wc_reset_variation_attr( 'sizes' );
			$product_img.wc_reset_variation_attr( 'title' );
			$product_img.wc_reset_variation_attr( 'alt' );
			$product_img.wc_reset_variation_attr( 'data-src' );
			$product_img.wc_reset_variation_attr( 'data-large_image' );
			$product_img.wc_reset_variation_attr( 'data-large_image_width' );
			$product_img.wc_reset_variation_attr( 'data-large_image_height' );
			$product_img_wrap.wc_reset_variation_attr( 'data-thumb' );
		}

		$( ".single_variation_wrap" ).on( "show_variation", function ( event, variation ) {
			var $product      = $variationsForm.closest( '.product' ),
			$product_gallery  = $product.find( '#seese-product-thumbnails-slider' ),
			$product_img_wrap = $product_gallery.find( '.woocommerce-product-gallery__image, woocommerce-product-gallery__image--placeholder' ).eq( 0 ),
			$product_img      = $product_img_wrap.find( '.wp-post-image' );

			if ( variation && variation.image && variation.image.src && variation.image.src.length > 1 ) {
				$product_img.wc_set_variation_attr( 'src', variation.image.src );
				$product_img.wc_set_variation_attr( 'height', variation.image.src_h );
				$product_img.wc_set_variation_attr( 'width', variation.image.src_w );
				$product_img.wc_set_variation_attr( 'srcset', variation.image.srcset );
				$product_img.wc_set_variation_attr( 'sizes', variation.image.sizes );
				$product_img.wc_set_variation_attr( 'title', variation.image.title );
				$product_img.wc_set_variation_attr( 'alt', variation.image.alt );
				$product_img.wc_set_variation_attr( 'data-src', variation.image.full_src );
				$product_img.wc_set_variation_attr( 'data-large_image', variation.image.full_src );
				$product_img.wc_set_variation_attr( 'data-large_image_width', variation.image.full_src_w );
				$product_img.wc_set_variation_attr( 'data-large_image_height', variation.image.full_src_h );
				$product_img_wrap.wc_set_variation_attr( 'data-thumb', variation.image.src );
			} else {
				$product_img.wc_reset_variation_attr( 'src' );
				$product_img.wc_reset_variation_attr( 'width' );
				$product_img.wc_reset_variation_attr( 'height' );
				$product_img.wc_reset_variation_attr( 'srcset' );
				$product_img.wc_reset_variation_attr( 'sizes' );
				$product_img.wc_reset_variation_attr( 'title' );
				$product_img.wc_reset_variation_attr( 'alt' );
				$product_img.wc_reset_variation_attr( 'data-src' );
				$product_img.wc_reset_variation_attr( 'data-large_image' );
				$product_img.wc_reset_variation_attr( 'data-large_image_width' );
				$product_img.wc_reset_variation_attr( 'data-large_image_height' );
				$product_img_wrap.wc_reset_variation_attr( 'data-thumb' );
			}
		});

		$variationsForm.on( 'click', '.reset_variations', function( event ) {
			resetVaritaion();
		});

		$( ".single_variation_wrap" ).on( "hide_variation", function ( event, variation ) {
      resetVaritaion();
		});

		////****************************************** My Account Page Scripts Starts ***************************************////

		$('#seese-woo-login-wrap').addClass('inline fade-in slide-up');

		var animTimeout = 250;

		function showRegisterForm() {
			var $loginWrap = $('#seese-woo-login-wrap');
			var $registerWrap = $('#seese-woo-register-wrap');

			$loginWrap.removeClass('fade-in');

			setTimeout(function() {
				$registerWrap.addClass('inline fade-in slide-up');
				$loginWrap.removeClass('inline slide-up');
			}, animTimeout);
		};

		function showLoginForm() {
			var $loginWrap = $('#seese-woo-login-wrap');
			var $registerWrap = $('#seese-woo-register-wrap');

			$registerWrap.removeClass('fade-in');

			setTimeout(function() {
				$loginWrap.addClass('inline fade-in slide-up');
				$registerWrap.removeClass('inline slide-up');
			}, animTimeout);
		};

		$('#seese-show-register-button').bind('click', function(e) {
			e.preventDefault();
			showRegisterForm();
		});

		$('#seese-show-login-button').bind('click', function(e) {
			e.preventDefault();
			showLoginForm();
		});

		////************************************* Others Script Starts ********************************////

		$('div.quantity.buttons_added .plus, td.quantity.buttons_added .plus').attr("value", $.parseHTML("&#xf0d8;")[0].data);
		$('div.quantity.buttons_added .minus, td.quantity.buttons_added .plus').attr("value", $.parseHTML("&#xf0d7;")[0].data);

	});

})(jQuery);