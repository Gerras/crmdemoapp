/**
 *	Neon Login Script
 *
 *	Developed by Arlind Nushi - www.laborator.co
 */

var neonLogin = neonLogin || {};

;(function($, window)
{
	"use strict";
	
	$(document).ready(function()
	{
		neonLogin.$container = $("#form_login");
		
		
		// Login Form & Validation
		neonLogin.$container.validate({
			rules: {
				username: {
					required: true	
				},
				
				password: {
					required: true
				}
				
			},
			
			highlight: function(element){
				$(element).closest(".input-group").addClass("validate-has-error");
			},
			
			
			unhighlight: function(element)
			{
				$(element).closest(".input-group").removeClass("validate-has-error");
			},
			
			submitHandler: function()
			{
				/* 
					Updated on v1.1.4
					Login form now processes the login data, here is the file: data/sample-login-form.php
				*/
				
				$(".login-page").addClass("logging-in"); // This will hide the login form and init the progress bar
					
					
				// Hide Errors
				$(".form-login-error").slideUp("fast");

				// We will wait till the transition ends				
				setTimeout(function()
				{
					var randomPct = 25 + Math.round(Math.random() * 30);
					
					// The form data are subbmitted, we can forward the progress to 70%
					neonLogin.setPercentage(40 + randomPct);
											
				    // Send data to the server
					var username = $("#username").val();
				    var password = $("#password").val();
					var dataobjects = {
					    Username: username,
					    Password: password
					};
					$.ajax({
					    method: "POST",
						url: "/Account/Login",
						contentType: "application/json; charset=utf-8",
						data: JSON.stringify(dataobjects),
						error: function()
						{
							alert("An error occoured!");
						},
						success: function(response)
						{
							// Login status [success|invalid]
							var loginStatus = response.login_status;
															
							// Form is fully completed, we update the percentage
							neonLogin.setPercentage(100);
							
							
							// We will give some time for the animation to finish, then execute the following procedures	
							setTimeout(function()
							{
								// If login is invalid, we store the 
								if(loginStatus === "invalid")
								{
									$(".login-page").removeClass("logging-in");
									neonLogin.resetProgressBar(true);
								}
								else
								if(loginStatus === "success")
								{
									// Redirect to login page
									setTimeout(function()
									{
										var redirectUrl = response.redirect_url;
										
										if(response.redirect_url && response.redirect_url.length)
										{
											redirectUrl = response.redirect_url;
										}
										
										window.location.href = redirectUrl;
									}, 400);
								}
								
							}, 1000);
						}
					});
						
					
				}, 650);
			}
		});
		
		
		
		
		// Lockscreen & Validation
		var isLockscreen = $(".login-page").hasClass("is-lockscreen");
		
		if(isLockscreen)
		{
			neonLogin.$container = $("#form_lockscreen");
			neonLogin.$ls_thumb = neonLogin.$container.find(".lockscreen-thumb");
			
			neonLogin.$container.validate({
				rules: {
				
					password: {
						required: true
					}
					
				},
				
				highlight: function(element){
					$(element).closest(".input-group").addClass("validate-has-error");
				},
				
				
				unhighlight: function(element)
				{
					$(element).closest(".input-group").removeClass("validate-has-error");
				},
				
				submitHandler: function()
				{	
					/* 
						Demo Purpose Only 
						
						Here you can handle the page login, currently it does not process anything, just fills the loader.
					*/
					
					$(".login-page").addClass("logging-in-lockscreen"); // This will hide the login form and init the progress bar
	
					// We will wait till the transition ends				
					setTimeout(function()
					{
						var randomPct = 25 + Math.round(Math.random() * 30);
						
						neonLogin.setPercentage(randomPct, function()
						{
							// Just an example, this is phase 1
							// Do some stuff...
							
							// After 0.77s second we will execute the next phase
							setTimeout(function()
							{
								neonLogin.setPercentage(100, function()
								{
									// Just an example, this is phase 2
									// Do some other stuff...
									
									// Redirect to the page
									setTimeout("window.location.href = '../../'", 600);
								}, 2);
								
							}, 820);
						});
						
					}, 650);
				}
			});
		}
		
		
		
		
		
		
		// Login Form Setup
		neonLogin.$body = $(".login-page");
		neonLogin.$login_progressbar_indicator = $(".login-progressbar-indicator h3");
		neonLogin.$login_progressbar = neonLogin.$body.find(".login-progressbar div");
		
		neonLogin.$login_progressbar_indicator.html("0%");
		
		if(neonLogin.$body.hasClass("login-form-fall"))
		{
			var focusSet = false;
			
			setTimeout(function() {
			    neonLogin.$body.addClass("login-form-fall-init");
				
				setTimeout(function()
				{
					if( !focusSet)
					{
						neonLogin.$container.find("input:first").focus();
						focusSet = true;
					}
					
				}, 550);
				
			}, 0);
		}
		else
		{
			neonLogin.$container.find("input:first").focus();
		}
		
		// Focus Class
		neonLogin.$container.find(".form-control").each(function(i, el)
		{
			var $this = $(el),
				$group = $this.closest(".input-group");
			
			$this.prev(".input-group-addon").click(function()
			{
				$this.focus();
			});
			
			$this.on({
				focus: function()
				{
					$group.addClass("focused");
				},
				
				blur: function()
				{
					$group.removeClass("focused");
				}
			});
		});
		
		// Functions
		$.extend(neonLogin, {
			setPercentage: function(pct, callback)
			{
				pct = parseInt(pct / 100 * 100, 10) + "%";
				
				// Lockscreen
				if(isLockscreen)
				{
					neonLogin.$lockscreen_progress_indicator.html(pct);
					
					var o = {
						pct: currentProgress
					};
					
					TweenMax.to(o, .7, {
						pct: parseInt(pct, 10),
						roundProps: ["pct"],
						ease: Sine.easeOut,
						onUpdate: function()
						{
							neonLogin.$lockscreen_progress_indicator.html(o.pct + "%");
							drawProgress(parseInt(o.pct, 10)/100);
						},
						onComplete: callback
					});	
					return;
				}
				
				// Normal Login
				neonLogin.$login_progressbar_indicator.html(pct);
				neonLogin.$login_progressbar.width(pct);
				
				var o = {
					pct: parseInt(neonLogin.$login_progressbar.width() / neonLogin.$login_progressbar.parent().width() * 100, 10)
				};
				
				TweenMax.to(o, .7, {
					pct: parseInt(pct, 10),
					roundProps: ["pct"],
					ease: Sine.easeOut,
					onUpdate: function()
					{
						neonLogin.$login_progressbar_indicator.html(o.pct + "%");
					},
					onComplete: callback
				});
			},
			
			resetProgressBar: function(displayErrors)
			{
				TweenMax.set(neonLogin.$container, {css: {opacity: 0}});
				
				setTimeout(function()
				{
					TweenMax.to(neonLogin.$container, .6, {css: {opacity: 1}, onComplete: function()
					{
						neonLogin.$container.attr("style", "");
					}});
					
					neonLogin.$login_progressbar_indicator.html("0%");
					neonLogin.$login_progressbar.width(0);
					
					if(displayErrors)
					{
						var $errorsContainer = $(".form-login-error");
						
						$errorsContainer.show();
						var height = $errorsContainer.outerHeight();
						
						$errorsContainer.css({
							height: 0
						});
						
						TweenMax.to($errorsContainer, .45, {css: {height: height}, onComplete: function()
						{
							$errorsContainer.css({height: "auto"});
						}});
						
						// Reset password fields
						neonLogin.$container.find("input[type=\"password\"]").val("");
					}
					
				}, 800);
			}
		});
		
		
		// Lockscreen Create Canvas
		if(isLockscreen)
		{
			neonLogin.$lockscreen_progress_canvas = $("<canvas></canvas>");
			neonLogin.$lockscreen_progress_indicator =  neonLogin.$container.find(".lockscreen-progress-indicator");
			
			neonLogin.$lockscreen_progress_canvas.appendTo(neonLogin.$ls_thumb);
			
			var thumbSize = neonLogin.$ls_thumb.width();
			
			neonLogin.$lockscreen_progress_canvas.attr({
				width: thumbSize,
				height: thumbSize
			});
			
			
			neonLogin.lockscreen_progress_canvas = neonLogin.$lockscreen_progress_canvas.get(0);
			
			// Create Progress Circle
			var bg = neonLogin.lockscreen_progress_canvas,
				ctx = ctx = bg.getContext("2d"),
				imd = null,
				circ = Math.PI * 2,
				quart = Math.PI / 2,
				currentProgress = 0;
			
			ctx.beginPath();
			ctx.strokeStyle = "#eb7067";
			ctx.lineCap = "square";
			ctx.closePath();
			ctx.fill();
			ctx.lineWidth = 3.0;
			
			imd = ctx.getImageData(0, 0, thumbSize, thumbSize);
			
			var drawProgress = function(current) {
			    ctx.putImageData(imd, 0, 0);
			    ctx.beginPath();
			    ctx.arc(thumbSize/2, thumbSize/2, 70, -(quart), ((circ) * current) - quart, false);
			    ctx.stroke();
			    
			    currentProgress = current * 100;
			}
			
			drawProgress(0/100);
			
			
			neonLogin.$lockscreen_progress_indicator.html("0%");
			
			ctx.restore();
		}
		
	});
	
})(jQuery, window);