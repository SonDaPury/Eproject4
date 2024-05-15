module.exports = {
  root: true,
  env: { browser: true, es2020: true },
  
  extends: [
    "eslint:recommended",
    "plugin:react/recommended",
    "plugin:react/jsx-runtime",
    "plugin:react-hooks/recommended",
  ],
  ignorePatterns: ["dist", ".eslintrc.cjs"],
  parserOptions: { ecmaVersion: "latest", sourceType: "module" },
  settings: { react: { version: "18.2" } },
  plugins: ["react-refresh", "react", "react-hooks"],
  rules: {
    "react/jsx-no-target-blank": "off",
    "react-refresh/only-export-components": [
      "warn",
      { allowConstantExport: true },
    ],
    "react-hooks/rules-of-hooks": "error",
    "react-hooks/exhaustive-deps": "warn",
    "react/prop-types": "off",
    "react/display-name": "off",
    "no-console": "warn",
    "no-lonely-if": "warn",
    "no-unused-vars": "warn",
    "no-trailing-spaces": "warn",
    "no-multi-spaces": "warn",
    "no-multiple-empty-lines": "warn",
    "space-before-blocks": ["error", "always"],
    "object-curly-spacing": ["error", "always"],
    "no-debugger": "off",  // Thêm dòng này
    indent: ["error", 2],
    semi: ["error", "always"],
    quotes: ["error", "double"],
    "array-bracket-spacing": "warn",
  },
  
};
