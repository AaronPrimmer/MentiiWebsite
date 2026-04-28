$(document).ready(function () {
    $('.like-button').click(async function () {
        var button = $(this);
        var id = this.dataset.postId;
        //console.log('Like button clicked for post ID:', JSON.stringify({ postId: id }));

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
                    'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val(),
                },
                body: JSON.stringify({ postId: id })
            });

            if (!response.ok) throw new Error("Request Failed");

            const data = await response.json();

            if (data.success == false) {
                button.toggleClass('liked'); // Revert visual state if like failed
            } else {
                button.find('.like-count').text(data.likeCount);
            }

            //console.log('Like response:', JSON.stringify(data));
        } catch (error) {
            console.error('Error liking the post:', error);
            button.toggleClass('liked');
        }
    });
});