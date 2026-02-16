mergeInto(LibraryManager.library, {
    DownloadCodeFile: function (filenamePtr, contentPtr) {
        const filename = UTF8ToString(filenamePtr);
        const content = UTF8ToString(contentPtr);

        const blob = new Blob([content], { type: "text/plain" });
        const url = URL.createObjectURL(blob);

        const a = document.createElement("a");
        a.href = url;
        a.download = filename;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);

        URL.revokeObjectURL(url);
    }
});
