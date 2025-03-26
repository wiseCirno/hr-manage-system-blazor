/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./**/*.{razor,cshtml}",
    ],
    theme: {
        extend: {},
    },
    plugins: [
        require('@tailwindcss/forms'),
    ],
}