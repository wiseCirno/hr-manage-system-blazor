/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./**/*.{razor,cshtml}",
    ],
    theme: {
        screens: {
            sm: "640px",
            md: "768px",
            lg: "1024px",
            xl: "1280px",
            "2xl": "1536px",
        },
    },
    plugins: [
        require('@tailwindcss/forms'),
    ],
}