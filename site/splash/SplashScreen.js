function onSourceDownloadProgressChanged(sender, eventArgs) {
    sender.findName("PercentageCounter").Text = Math.round((eventArgs.progress * 100), 0).toString() + "%";
    sender.findName("rotateTransform").Angle = eventArgs.progress * 100 * 3.6 * 2;
}