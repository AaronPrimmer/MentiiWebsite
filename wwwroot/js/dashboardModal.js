$(document).ready(function() {
    $("#postBtnSave").click(function() {
        var formData = $("#addPostForm").serialize(); // Collects input values
        $.ajax({
            type: "POST",
            url: "/Post/Create", // Ensure this matches your Controller/Action
            data: formData,
            success: function(response) {
                if (response.success) {
                    $('#addPostModal').modal('hide'); // Close the modal
                    location.reload(); // Optional: Refresh to see changes
                } else {
                    alert("Error: " + response.message);
                }
            },
            error: function() {
                alert("Error while saving post.");
            }
        });
    });
});