export function beforeStart(options, extensions) {


    console.log("beforeStart", options, extensions);
}

export function afterStarted(blazor) {
    console.log("afterStarted", blazor);
}
