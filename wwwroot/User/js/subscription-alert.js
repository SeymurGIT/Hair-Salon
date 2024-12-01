<script>
    $(document).ready(function() {
        // Change the duration (in milliseconds) as needed
        var duration = 5000; // 5 seconds in this example

    // Function to hide the alert after 'duration' milliseconds
    setTimeout(function() {
        $("#successAlert").fadeOut("slow");
        }, duration);
    });
</script>
