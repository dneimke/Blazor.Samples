export async function enableImagePaste(componentInstance) {

    window.addEventListener("paste", async (e) => {

        try {
            let content = await getClipboardImage();

            if (!!content && !!content.blob) {
                const fileExtension = content.type.split("/").pop();
                const formData = new FormData();
                formData.append("pastedImage", content.blob, `pastedImage.${fileExtension}`);

                const headers = {}
                const response = await fetch("/api/images/paste/save", {
                    method: 'POST',
                    body: formData,
                    headers: headers
                });

                if (response.ok) {
                    const json = await response.json()
                    const url = json.imageUrl;
                    await componentInstance.invokeMethodAsync('HandlePaste', url);
                } else {
                    alert('An error occurred while attempting to save the pasted image')
                }
            }
        } catch (error) {

        }
        
    }, false);
}

async function getClipboardImage() {
    const permissions = await navigator.permissions.query({ name: "clipboard-read" });

    if (permissions.state == "granted" || permissions.state == "prompt") {
        try {
            const items = await navigator.clipboard.read();
            const type = items[0].types[0];
            if (type.indexOf("image") == -1) return;

            const blob = await items[0].getType(type);
            return { blob, type };

        } catch (err) {
            console.error(err.name, err.message);
        }
    }

    return null;
}