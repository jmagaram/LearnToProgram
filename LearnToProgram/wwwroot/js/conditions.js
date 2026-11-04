// Small helpers called from the C# code on the Conditions page.
window.practiceStorage = {
    load: key => { try { return localStorage.getItem(key); } catch (e) { return null; } },
    save: (key, value) => { try { localStorage.setItem(key, value); } catch (e) { } },
};

window.practiceScrollTo = id => {
    const el = document.getElementById(id);
    if (el) { el.scrollIntoView({ behavior: "smooth", block: "start" }); history.replaceState(null, "", "#" + id); }
};

// Code boxes: Tab indents, Ctrl+Enter / Cmd+Enter runs that problem.
document.addEventListener("keydown", e => {
    const box = e.target;
    if (!(box instanceof HTMLTextAreaElement) || !box.classList.contains("answer")) return;
    if (e.key === "Enter" && (e.ctrlKey || e.metaKey)) {
        e.preventDefault();
        box.closest(".card").querySelector("button.run").click();
    } else if (e.key === "Tab" && !e.shiftKey) {
        e.preventDefault();
        const start = box.selectionStart, end = box.selectionEnd;
        box.value = box.value.slice(0, start) + "    " + box.value.slice(end);
        box.selectionStart = box.selectionEnd = start + 4;
        box.dispatchEvent(new Event("input", { bubbles: true }));
    }
});
