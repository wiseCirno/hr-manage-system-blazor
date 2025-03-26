window.downloadFileFromBytes = (fileName, contentType, byteBase64) => {
    const blob = new Blob([byteBase64], {type: contentType});
    const link = document.createElement('a');
    link.href = window.URL.createObjectURL(blob);
    link.download = fileName;
    link.click();
    window.URL.revokeObjectURL(link.href);
};
