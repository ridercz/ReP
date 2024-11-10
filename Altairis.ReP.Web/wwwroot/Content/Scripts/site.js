// Initialize validation service
var v = new aspnetValidation.ValidationService();
v.bootstrap();

// Initialize GET to POST conversion
document.querySelectorAll('a[data-convert-to-post]')
    .forEach(a => {
        // Check if there is onclick attribute
        if (a.hasAttribute('onclick')) {
            // Get onclick attribute value
            const onclick = a.getAttribute('onclick');

            // Check if onlic is in form of "return confirm('...')"
            if (onclick.startsWith("return confirm('" && onclick.endsWith(');'))) {
                // Get message from confirm
                const message = onclick.substring(17, onclick.length - 2);

                // Remove attribute and replace with custom confirm message
                a.removeAttribute('onclick');
                a.setAttribute("data-post-confirm", message)
            }
        }

        a.addEventListener('click', e => {
            e.preventDefault();

            // Check if confirmation is required
            const message = a.getAttribute('data-post-confirm');
            if (message && !confirm(message)) return;

            // Get CSRF token from __RequestVerificationToken
            const token = document.querySelector('input[name="__RequestVerificationToken"]');
            if (!token) {
                console.error('CSRF token not found');
                return;
            }

            // Create form and submit it
            const form = document.createElement('form');
            form.method = 'post';
            form.action = a.getAttribute('href');
            form.innerHTML = token.outerHTML;
            document.body.appendChild(form);
            form.submit();
        });
    });
