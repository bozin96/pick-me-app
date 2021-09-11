module.exports = {
  env: {
    browser: true,
    es2021: true,
  },
  extends: [
    'plugin:react/recommended',
    'airbnb', 'plugin:@typescript-eslint/recommended',
  ],
  parser: '@typescript-eslint/parser',
  parserOptions: {
    ecmaFeatures: {
      jsx: true,
    },
    ecmaVersion: 12,
    sourceType: 'module',
  },
  root: true,
  plugins: [
    'react',
    '@typescript-eslint',
    'react-hooks',
  ],
  rules: {
    'no-use-before-define': 'off',
    indent: 'off',
    'react/jsx-indent': 'off',
    'react/jsx-indent-props': 'off',
    '@typescript-eslint/no-use-before-define': ['error'],
    'react/jsx-filename-extension': ['warn', { extensions: ['.tsx'] }],
    'no-shadow': 'off',
    '@typescript-eslint/no-shadow': ['error'],
    '@typescript-eslint/explicit-function-return-type': [
      'error',
      {
        allowExpressions: true,
      },
    ],
    'react-hooks/rules-of-hooks': 'error',
    'react-hooks/exhaustive-deps': 'warn',
    'import/prefer-default-export': 'off',
    'linebreak-style': 0,
    'import/no-unresolved': [
      2,
      { caseSensitive: false },
    ],
    'import/extensions': [
      'error',
      'ignorePackages',
      {
        js: 'never',
        jsx: 'never',
        ts: 'never',
        tsx: 'never',
      },
    ],
  },
  settings: {
    'import/resolver': {
      node: {
        extensions: ['.js', '.jsx', '.ts', '.tsx'],
      },
    },
  },
};
