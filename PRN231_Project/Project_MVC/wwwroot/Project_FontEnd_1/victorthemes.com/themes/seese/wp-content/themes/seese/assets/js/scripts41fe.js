/*
 * All Plugins Used in this Seese Theme
 * Author & Copyright: VictorThemes
 * URL: https://victorthemes.com
 */


(function($){

	'use strict';

	$(window).load(function() {

		$('.seese-preloader-mask').delay(200).fadeOut();
    $('#preloader').delay(350).fadeOut('slow');
    $('body').delay(350).css({'overflow':'visible'});

		// OWL Carousel For Single Post Slider Images
		$('.seese-blog-post .seese-featureImg-carousel').owlCarousel({
			items: 1,
			loop: true,
			nav: true,
			dots: false,
			autoplay: false,
			autoHeight: true,
			navText: ["<i class='fa fa-angle-left' aria-hidden='true'></i>", "<i class='fa fa-angle-right' aria-hidden='true'></i>"],
			responsive: {
				0: {
					items: 1
				},
				600: {
					items: 1
				}
			}
		});

	});

	$(document).ready(function() {

		var isIE = document.body.style.msTouchAction !== undefined;
		if(isIE) {
			$('html').addClass('ie10');
		} else {
			$('html').addClass('seese-browser');
		}

		// Slicknav Mobile Menu
		$("#seese-menu").slicknav({
			label: '',
			duplicate: true,
			nestedParentLinks: true,
			duration: 200,
			allowParentLinks: true,
			prependTo: "#seese-mobilemenu"
		});

		// Products Click
		$('.product').on('click touchend', function(e) {
			var el = $(this);
			$(el).trigger('hover');
		});

		// Magnific Popup Gallery
		$(".seese-gallery").magnificPopup({
			delegate: 'a',
			type: 'image',
			closeOnContentClick: false,
			closeBtnInside: false,
			mainClass: 'mfp-with-zoom',
			image: {
				verticalFit: true,
			},
			gallery: {
				enabled: true,
			},
			zoom: {
				enabled: true,
				duration: 300,
				opener: function(element) {
					return element.find("img");
				}
			}
		});

		// Magnific Popup Single Image
		$(".seese-img-popup").magnificPopup({
			type: 'image',
			closeOnContentClick: false,
			closeOnBgClick: true,
			closeBtnInside: false,
			mainClass: 'mfp-with-zoom',
			image: {
				verticalFit: true
			},
			zoom: {
				enabled: true,
				duration: 300,
				opener: function(element) {
					return element.find("img");
				}
			}
		});

		// Scrolling Header
		$(window).scroll(function() {
			var scroll = $(window).scrollTop();
			if (scroll >= 20) {
				$(".seese-header").addClass("scrolling");
			} else {
				$(".seese-header").removeClass("scrolling");
			}
		});

		// Go To Top
		var duration = 500;
		$('.seese-gototop a').click(function(event) {
			event.preventDefault();
			$('html, body').animate({
				scrollTop: 0
			}, duration);
			return false;
		})

		// FitJs Video
		$(".seese-content-area").fitVids();

		// Blog Masonary Call
		var $blog_masonry_container;
		if ($('.seese-blog-wrapper').hasClass('seese-masonry-blog')) {
			$blog_masonry_container = $('.seese-blog-msnry');
			$blog_masonry_container.imagesLoaded(function() {
				$blog_masonry_container.masonry({
					itemSelector: '.seese-blog-msnry-item',
					columnWidth: '.seese-blog-msnry-sizer',
				});
			});
		}

		// OWL Carousel Blog Feature Image
		$('.seese-blog-wrapper .seese-featureImg-carousel').owlCarousel({
			items: 1,
			loop: true,
			nav: true,
			dots: false,
			autoplay: false,
			autoHeight: false,
			navText: ["<i class='fa fa-angle-left' aria-hidden='true'></i>", "<i class='fa fa-angle-right' aria-hidden='true'></i>"],
			responsive: {
				0: {
					items: 1
				},
				600: {
					items: 1
				}
			}
		});

		// Blog Post Ajax Load
		var $nextPageLinkBlog = $('.seese-blog-load-more-link').find('a');
		var $loadMoreControlsBlog = $('.seese-blog-load-more-controls');
		var nextPageUrlBlog = $nextPageLinkBlog.attr('href');

		if (nextPageUrlBlog) {
			$('.seese-blog-load-more-controls #seese-loaded').addClass('seese-link-present');
		} else {
			$loadMoreControlsBlog.addClass('seese-hide-btn');
		}

		$('.seese-blog-wrapper #seese-blog-load-more-btn').on('click', function(e) {
			e.preventDefault();

			if (nextPageUrlBlog) {
				if ($loadMoreControlsBlog.hasClass('seese-hide-btn')) {
					$loadMoreControlsBlog.removeClass('seese-hide-btn');
				}
				$loadMoreControlsBlog.addClass('seese-loader');

				$.ajax({
					url: nextPageUrlBlog,
					dataType: 'html',
					method: 'GET',
					error: function(XMLHttpRequest, textStatus, errorThrown) {
						console.log('SEESE: AJAX error - ' + errorThrown);
					},
					complete: function() {
						$loadMoreControlsBlog.removeClass('seese-loader');
					},
					success: function(response) {
						if ($('.seese-blog-wrapper').hasClass('seese-masonry-blog')) {
							var $newElements = $($.parseHTML(response)).find('.seese-blogs').children('.seese-blog-msnry-item');
							$blog_masonry_container.append($newElements).imagesLoaded(function() {
								$blog_masonry_container.masonry('appended', $newElements);
							});
						} else {
							var $newElements = $($.parseHTML(response)).find('.seese-blogs').children('.row');
							$('.seese-blog-wrapper').find('.seese-blogs').append($newElements);
						}

						// Magnific Popup Gallery
						$(".seese-gallery").magnificPopup({
							delegate: 'a',
							type: 'image',
							closeOnContentClick: false,
							closeBtnInside: false,
							mainClass: 'mfp-with-zoom',
							image: {
								verticalFit: true,
							},
							gallery: {
								enabled: true,
							},
							zoom: {
								enabled: true,
								duration: 300,
								opener: function(element) {
									return element.find("img");
								}
							}
						});

						// Magnific Popup Single Image
						$(".seese-img-popup").magnificPopup({
							type: 'image',
							closeOnContentClick: false,
							closeOnBgClick: true,
							closeBtnInside: false,
							mainClass: 'mfp-with-zoom',
							image: {
								verticalFit: true
							},
							zoom: {
								enabled: true,
								duration: 300,
								opener: function(element) {
									return element.find("img");
								}
							}
						});

						$('.seese-blog-wrapper .seese-featureImg-carousel').owlCarousel({
							items: 1,
							loop: true,
							nav: true,
							dots: false,
							autoplay: false,
							autoHeight: false,
							navText: ["<i class='fa fa-angle-left' aria-hidden='true'></i>", "<i class='fa fa-angle-right' aria-hidden='true'></i>"],
							responsive: {
								0: {
									items: 1
								},
								600: {
									items: 1
								}
							}
						});

	                    $(".seese-content-area").fitVids();

						nextPageUrlBlog = $($.parseHTML(response)).find('.seese-blog-load-more-link').children('a').attr('href');
						if (nextPageUrlBlog) {
							$nextPageLinkBlog.attr('href', nextPageUrlBlog);
						} else {
							$loadMoreControlsBlog.addClass('seese-all-loaded');
							$nextPageLinkBlog.removeAttr('href');
							$('.seese-blog-load-more-controls #seese-loaded').removeClass('seese-link-present');
						}
					}
				});
			} else {
				$loadMoreControlsBlog.addClass('seese-hide-btn');
			}
		});

		// Order Tracking form custom div wrap
		$(".woocommerce .track_order p.form-row").wrapAll('<div class="seese-form-less-width"></div>');

		// Wish list for out of stock product
		if ($( ".single-product p" ).hasClass( "out-of-stock" ) ){
			$( ".yith-wcwl-add-to-wishlist, .yith-wcwl-add-button" ).addClass('wish-show');
		}

		// Add class for clear link
		$( ".variations .reset_variations" ).addClass('button');



		// $(".woocommerce.single table.variations td select").append('<i class="fa fa-angle-down" aria-hidden="true"></i>');
		$('<i class="fa fa-angle-down" aria-hidden="true"></i>').insertAfter('.woocommerce.single table.variations td select');


	});

	$(window).scroll(function() {

		if( ( $(window).scrollTop() + $(window).height() ) < ( $(document).height() - ( $(".seese-footer").height() + $(".seese-footer").height() ) ) ) {
	        $('.seese-gototop').css({ 'opacity' : 1 }).css({ 'visibility' : 'visible' });
	        $('.seese-specialPage').css({ 'opacity' : 1 }).css({ 'visibility' : 'visible' });
		}
	    if( ( $(window).scrollTop() + $(window).height() ) > ( $(document).height() - ( $(".seese-footer").height() + $(".seese-footer").height() ) ) ) {
      		$('.seese-gototop').css({ 'opacity' : 0 }).css({ 'visibility' : 'hidden' });
      		$('.seese-specialPage').css({ 'opacity' : 0 }).css({ 'visibility' : 'hidden' });
	 	}

	});

			// WPML Dropdown
		$('.seese-topdd-content').hide();
    $('.seese-top-dropdown').each(function() {

      var $this    = $(this),
          $open    = $this.find('.seese-top-active'),
          $content = $this.find('.seese-topdd-content');

      $open.on('click', function( e ) {

        e.preventDefault();
        e.stopPropagation();

				if ($( ".seese-top-active i" ).hasClass( "fa-angle-down" ) ){
					$( ".seese-top-active i" ).removeClass('fa-angle-down');
					$( ".seese-top-active i" ).addClass('fa-angle-up');
				} else {
				  $( ".seese-top-active i" ).addClass('fa-angle-down');
				  $( ".seese-top-active i" ).removeClass('fa-angle-up');
				}
				$(document.body).on('click', function () {
		      $( ".seese-top-active i" ).addClass('fa-angle-down');
				  $( ".seese-top-active i" ).removeClass('fa-angle-up');
		    });

        if( $content.hasClass('seese-opened') ) {
          $content.removeClass('seese-opened').fadeOut('fast');
        } else {
          $content.trigger('close-modals').addClass('seese-opened').fadeIn('fast');
          $content.find('input').focus();
        }

      });

      $content.on('click', function ( event ) {

        if (event.stopPropagation) {
          event.stopPropagation();
        } else if ( window.event ) {
          window.event.cancelBubble = true;
        }

      });

      $(document.body).on('click close-modals', function () {
	      $('.seese-topdd-content').removeClass('seese-opened').fadeOut('fast');
	    });

    });

})(jQuery);
