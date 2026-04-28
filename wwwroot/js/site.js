$("#profileForm").submit(function (event) {
    event.preventDefault(); // Prevent default form submission

    const userName = $("#usernameField").val().trim();
    if (userName) {
        window.location.href = `/User/Profile/${encodeURIComponent(userName)}`;
    }
});