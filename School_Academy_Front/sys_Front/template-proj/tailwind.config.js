/** @type {import('tailwindcss').Config} */
module.exports = {
  // ─── Dark Mode via class on <html> ────────────────────────────────────────
  darkMode: "class",

  // ─── Content Scanning ─────────────────────────────────────────────────────
  content: ["./index.html", "./src/**/*.{vue,js,ts,jsx,tsx}"],

  // ─── Theme Extension ──────────────────────────────────────────────────────
  theme: {
    extend: {
      // --- Fonts (حسب 01-theme-colors.md) ---
      fontFamily: {
        outfit: ["Outfit", "Cairo", "system-ui", "-apple-system", "sans-serif"],
        cairo: ["Cairo", "Outfit", "system-ui", "-apple-system", "sans-serif"],
      },

      // --- Brand Colors (Primary) ---
      colors: {
        brand: {
          25: "#f2f7ff",
          50: "#ecf3ff",
          100: "#dde9ff",
          200: "#c2d6ff",
          300: "#9cb9ff",
          400: "#7592ff",
          500: "#465fff",
          600: "#3641f5",
          700: "#2a31d8",
          800: "#252dae",
          900: "#262e89",
          950: "#161950",
        },
        // --- Gray (Secondary + Surfaces) ---
        gray: {
          25: "#fcfcfd",
          50: "#f9fafb",
          100: "#f2f4f7",
          200: "#e4e7ec",
          300: "#d0d5dd",
          400: "#98a2b3",
          500: "#667085",
          600: "#475467",
          700: "#344054",
          800: "#1d2939",
          900: "#101828",
          950: "#0c111d",
          dark: "#1a2231",
        },
        // --- Success ---
        success: {
          25: "#f6fef9",
          50: "#ecfdf3",
          100: "#d1fadf",
          200: "#a6f4c5",
          300: "#6ce9a6",
          400: "#32d583",
          500: "#12b76a",
          600: "#039855",
          700: "#027a48",
          800: "#05603a",
          900: "#054f31",
          950: "#032d1e",
        },
        // --- Error ---
        error: {
          25: "#fffbfa",
          50: "#fef3f2",
          100: "#fee4e2",
          200: "#fecdca",
          300: "#fda29b",
          400: "#f97066",
          500: "#f04438",
          600: "#d92d20",
          700: "#b42318",
          800: "#912018",
          900: "#7a271a",
          950: "#55160c",
        },
        // --- Warning ---
        warning: {
          25: "#fffcf5",
          50: "#fffaeb",
          100: "#fef0c7",
          200: "#fedf89",
          300: "#fec84b",
          400: "#fdb022",
          500: "#f79009",
          600: "#dc6803",
          700: "#b54708",
          800: "#93370d",
          900: "#7a2e0e",
          950: "#4e1d09",
        },
        // --- Blue Light (Info/Accent) ---
        "blue-light": {
          25: "#f5fbff",
          50: "#f0f9ff",
          100: "#e0f2fe",
          200: "#b9e6fe",
          300: "#7cd4fd",
          400: "#36bffa",
          500: "#0ba5ec",
          600: "#0086c9",
          700: "#026aa2",
          800: "#065986",
          900: "#0b4a6f",
          950: "#062c41",
        },
      },

      // --- Border Radius ---
      borderRadius: {
        // حسب جدول 01-theme-colors.md
        btn: "10px",
        card: "12px",
      },

      // --- Box Shadows ---
      boxShadow: {
        // shadow-theme-* (حسب 01-theme-colors.md)
        "theme-xs": "0 1px 2px rgba(16, 24, 40, 0.05)",
        "theme-sm":
          "0 1px 3px rgba(16, 24, 40, 0.1), 0 1px 2px rgba(16, 24, 40, 0.06)",
        "theme-md":
          "0 4px 8px -2px rgba(16, 24, 40, 0.1), 0 2px 4px -2px rgba(16, 24, 40, 0.06)",
        "theme-lg":
          "0 12px 16px -4px rgba(16, 24, 40, 0.08), 0 4px 6px -2px rgba(16, 24, 40, 0.03)",
        "theme-xl":
          "0 20px 24px -4px rgba(16, 24, 40, 0.08), 0 8px 8px -4px rgba(16, 24, 40, 0.03)",
        "focus-ring": "0 0 0 4px rgba(70, 95, 255, 0.12)",
      },

      // --- Breakpoints (حسب 01-theme-colors.md) ---
      screens: {
        "2xsm": "375px",
        xsm: "425px",
        "3xl": "2000px",
      },

      // --- Z-index ---
      zIndex: {
        99999: "99999",
        999: "999",
        100: "100",
        99: "99",
      },
    },
  },

  plugins: [],
};
