/**
 *	Neon Register Script
 *
 *	Developed by Arlind Nushi - www.laborator.co
 */

var neonForgotPassword = neonForgotPassword || {};

;(function($)
{
	"use strict";
	
	$(document).ready(function()
	{
		neonForgotPassword.$container = $("#form_forgot_password");
		neonForgotPassword.$steps = neonForgotPassword.$container.find(".form-steps");
		neonForgotPassword.$steps_list = neonForgotPassword.$steps.find(".step");
		neonForgotPassword.step = "step-1"; // current step
		
				
		neonForgotPassword.$container.validate({
			rules: {
				username: {
					required: true,
					email: true
				},
                firstname: {
                    required: true
                },
                lastname: {
                    required: true
                },
                password: {
                    required: true,
                    minlength : 5
                },
                password2: {
                    required: true,
                    minlength: 5,
                    equalTo: "#password"
                }
			},
			
			messages: {
				
				username: {
					email: "Invalid E-mail."
				},
                password2: {
                    equalTo: "Passwords do not match."
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
				//$(".login-page").addClass("logging-in");
				
				// We consider its 30% completed form inputs are filled
				//neonForgotPassword.setPercentage(30, function()
				//{
				//	// Lets move to 98%, meanwhile ajax data are sending and processing
				//	neonForgotPassword.setPercentage(98, function()
				//	{
			    // Send data to the server
				var username = $("#username").val();
				var firstname = $("#firstname").val();
				var lastname = $("#lastname").val();
				var password = $("#password").val();
				var confirm = $("#password2").val();
			    var dataobjects = {
			        UserName: username,
			        FirstName: firstname,
			        LastName: lastname,
			        Password: password,
			        ConfirmPassword: confirm
			    };
				        $.ajax({
				            method: "POST",
				            url: "/Account/Register",
						    contentType: "application/json; charset=utf-8",

							data: JSON.stringify(dataobjects),
							error: function()
							{
								alert("An error occured!");
							},
							success: function(response)
							{
							    if (response.success) {
							        $("#form_title").html(response.message);
							        setTimeout(function() {
							            window.location.href = "/Home/ViewContacts";
							        }, 1000);
							    } else {
							        
							        // From response you can fetch the data object retured
							        //var email = response.submitted_data.email;
							        var stringFormater = "";
							        $.each(response.message, function (index, value) {
							            stringFormater += "<p>" + (index + 1) + ": " + value + "</p>";
							        });
							        $("#form_title").html(stringFormater);
							        $("html, body").animate({ scrollTop: $(document).height() }, "slow");

							        // Form is fully completed, we update the percentage
							        //neonForgotPassword.setPercentage(100);
							    }
								// We will give some time for the animation to finish, then execute the following procedures	
								//setTimeout(function() {
								//    window.location.href = "/Home/ViewContacts";
								//}, 1000);
							}
						});
				//	});
				//});
			}
		});
	
		// Steps Handler
		neonForgotPassword.$steps.find("[data-step]").on("click", function(ev)
		{
			ev.preventDefault();
			
			var $currentStep = neonForgotPassword.$steps_list.filter(".current"),
				nextStep = $(this).data("step"),
				validator = neonForgotPassword.$container.data("validator"),
				errors = 0;
			
			neonForgotPassword.$container.valid();
			errors = validator.numberOfInvalids();
			
			if(errors)
			{
				validator.focusInvalid();
			}
			else
			{
				var $nextStep = neonForgotPassword.$steps_list.filter("#" + nextStep),
					$otherSteps = neonForgotPassword.$steps_list.not( $nextStep ),
					
					currentStepHeight = $currentStep.data("height"),
					nextStepHeight = $nextStep.data("height");
				
				TweenMax.set(neonForgotPassword.$steps, {css: {height: currentStepHeight}});
				TweenMax.to(neonForgotPassword.$steps, 0.6, {css: {height: nextStepHeight}});
				
				TweenMax.to($currentStep, .3, {css: {autoAlpha: 0}, onComplete: function()
				{
					$currentStep.attr("style", "").removeClass("current");
					
					var $formElements = $nextStep.find(".form-group");
					
					TweenMax.set($formElements, {css: {autoAlpha: 0}});
					$nextStep.addClass("current");
					
					$formElements.each(function(i, el)
					{
						var $formElement = $(el);
						
						TweenMax.to($formElement, .2, {css: {autoAlpha: 1}, delay: i * .09});
					});
					
					setTimeout(function()
					{
						$formElements.add($nextStep).add($nextStep).attr("style", "");
						$formElements.first().find("input").focus();
						
					}, 1000 * (.5 + ($formElements.length - 1) * .09));
				}});
			}
		});
		
		neonForgotPassword.$steps_list.each(function(i, el)
		{
			var $this = $(el),
				isCurrent = $this.hasClass("current"),
				margin = 20;
			
			if(isCurrent)
			{
				$this.data("height", $this.outerHeight() + margin);
			}
			else
			{
				$this.addClass("current").data("height", $this.outerHeight() + margin).removeClass("current");
			}
		});
		
		
		// Login Form Setup
		neonForgotPassword.$body = $(".login-page");
		neonForgotPassword.$login_progressbar_indicator = $(".login-progressbar-indicator h3");
		neonForgotPassword.$login_progressbar = neonForgotPassword.$body.find(".login-progressbar div");
		
		neonForgotPassword.$login_progressbar_indicator.html("0%");
		
		if(neonForgotPassword.$body.hasClass("login-form-fall"))
		{
			var focusSet = false;
			
			setTimeout(function() {
			    neonForgotPassword.$body.addClass("login-form-fall-init");
				
				setTimeout(function()
				{
					if( !focusSet)
					{
						neonForgotPassword.$container.find("input:first").focus();
						focusSet = true;
					}
					
				}, 550);
				
			}, 0);
		}
		else
		{
			neonForgotPassword.$container.find("input:first").focus();
		}
		
		
		// Functions
		$.extend(neonForgotPassword, {
			setPercentage: function(pct, callback)
			{
				pct = parseInt(pct / 100 * 100, 10) + "%";
				
				// Normal Login
				neonForgotPassword.$login_progressbar_indicator.html(pct);
				neonForgotPassword.$login_progressbar.width(pct);
				
				var o = {
					pct: parseInt(neonForgotPassword.$login_progressbar.width() / neonForgotPassword.$login_progressbar.parent().width() * 100, 10)
				};
				
				TweenMax.to(o, .7, {
					pct: parseInt(pct, 10),
					roundProps: ["pct"],
					ease: Sine.easeOut,
					onUpdate: function()
					{
						neonForgotPassword.$login_progressbar_indicator.html(o.pct + "%");
					},
					onComplete: callback
				});
			}
		});
	});
	
})(jQuery, window);