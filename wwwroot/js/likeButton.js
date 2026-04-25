$(document).ready(function () {
    $('.like-button').click(async function () {
        var button = $(this);
        var id = button.data('post-id');
        console.log('Like button clicked for post ID:', id);

        // Optional: Toggle visual state immediately
        button.toggleClass('liked');
        button.addClass('pop');

        // Remove animation class after it finishes
        setTimeout(() => button.removeClass('pop'), 300);

        try {
            const response = await fetch('/Post/Like', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                },
                body: JSON.stringify({ postId: id })
            });

            if (!response.ok) throw new Error("Request Failed");

            const data = await response.json();

            console.log('Like response:', JSON.stringify(data));

            button.find('.like-count').text(data.likeCount);
        } catch (error) {
            console.error('Error liking the post:', error);
            button.toggleClass('liked');
        }
    });
});