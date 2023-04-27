
$('.print').on('click', function() { // select print button with class "print," then on click run callback function
$.print(".content"); // inside callback function the section with class "content" will be printed
});

// plugin creator and full list of options: https://github.com/DoersGuild/jQuery.print