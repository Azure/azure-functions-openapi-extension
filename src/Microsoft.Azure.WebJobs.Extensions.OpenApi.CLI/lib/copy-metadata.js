const fs = require('fs');
const path = require('path');

const readMeSrc = path.resolve(__dirname, '..', '..', '..', '..', 'docs', 'openapi-cli.md');
const readMeDest = path.resolve(__dirname, '..', 'README.md');

fs.copyFile(readMeSrc, readMeDest, (err) => {
    if (err) throw err;
    console.log('Copied openapi-cli.md from the docs directory.');
});