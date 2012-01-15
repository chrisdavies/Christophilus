var toph = toph || {};
toph.error = function (m) {
    return toph.error.parse(m).message;
};

toph.error.parse = function (m) {
    if (m.responseText.charAt(0) == '{') {
        return eval('(' + m.responseText + ')');
    } else {
        return { message: 'An error occurred while processing your request.' };
    }
}