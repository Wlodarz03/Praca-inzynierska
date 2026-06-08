mergeInto(LibraryManager.library, {
    CopyToClipboardJS: function (text) {
        var str = UTF8ToString(text);
        
        var elem = document.createElement('textarea');
        elem.value = str;
        
        elem.style.top = "0";
        elem.style.left = "0";
        elem.style.position = "fixed";

        document.body.appendChild(elem);
        elem.focus();
        elem.select();

        try {
            var successful = document.execCommand('copy');
            var msg = successful ? 'successful' : 'unsuccessful';
            console.log('Copying text command was ' + msg);
        } catch (err) {
            console.error('Oops, unable to copy', err);
        }

        document.body.removeChild(elem);
    }
});