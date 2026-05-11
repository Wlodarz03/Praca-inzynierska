mergeInto(LibraryManager.library, {
    CopyToClipboardJS: function (text) {
        // Konwersja wskaźnika tekstowego z C# na string w JS (nowy standard Unity)
        var str = UTF8ToString(text);
        
        // Tworzymy ukryty element tekstowy
        var elem = document.createElement('textarea');
        elem.value = str;
        
        // Unikamy przewijania strony na dół
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