$(document).ready(function () {
    $('#passwordInput').focus(function () {
        $(this).removeClass('input-validation-error');
        $('span[data-valmsg-for="Password"]').empty();
    });
    $('#login').focus(function () {
        $(this).removeClass('input-validation-error');
        $('span[data-valmsg-for="UserName"]').empty();
    });
});

function togglePasswordVisibility() {
    var passwordInput = $("#passwordInput");
    var passwordIcon = $("#password-icon");

    if (passwordInput.attr("type") === "password") {
        passwordInput.attr("type", "text");
        passwordIcon.removeClass("fa-eye-slash").addClass("fa-eye");
    } else {
        passwordInput.attr("type", "password");
        passwordIcon.removeClass("fa-eye").addClass("fa-eye-slash");
    }
}
