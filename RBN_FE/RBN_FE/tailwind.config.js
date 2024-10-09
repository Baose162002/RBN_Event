module.exports = {
    content: [
        './Pages/**/*.cshtml',
        './Views/**/*.cshtml'
    ],
    darkMode: 'class',
    theme: {
        extend: {
            fontFamily: { Poppins: ["Poppins", "sans-serif"] }
        },
    },
    plugins: [require('tailwindcss-animate')],
}