// Small helpers called from the C# code on the practice pages.
window.practiceStorage = {
    load: key => { try { return localStorage.getItem(key); } catch (e) { return null; } },
    save: (key, value) => { try { localStorage.setItem(key, value); } catch (e) { } },
};

window.practiceScrollTo = id => {
    const el = document.getElementById(id);
    if (el) { el.scrollIntoView({ behavior: "smooth", block: "start" }); history.replaceState(null, "", location.pathname + location.search + "#" + id); }
};

// Code boxes: Tab indents, Enter keeps the indent, Ctrl+Enter / Cmd+Enter runs that problem.
document.addEventListener("keydown", e => {
    const box = e.target;
    if (!(box instanceof HTMLTextAreaElement) || !box.classList.contains("answer")) return;
    if (e.key === "Enter" && (e.ctrlKey || e.metaKey)) {
        e.preventDefault();
        box.closest(".card").querySelector("button.run").click();
    } else if (e.key === "Enter" && !e.shiftKey && !e.altKey) {
        // Keep the indent of the line above, and indent one more step after a {
        e.preventDefault();
        const start = box.selectionStart, end = box.selectionEnd;
        const line = box.value.slice(box.value.lastIndexOf("\n", start - 1) + 1, start);
        let indent = line.match(/^\s*/)[0];
        if (line.trimEnd().endsWith("{")) indent += "    ";
        box.value = box.value.slice(0, start) + "\n" + indent + box.value.slice(end);
        box.selectionStart = box.selectionEnd = start + 1 + indent.length;
        box.dispatchEvent(new Event("input", { bubbles: true }));
    } else if (e.key === "}" && !e.ctrlKey && !e.metaKey) {
        // A } typed on a line with only spaces moves back one indent step.
        const start = box.selectionStart;
        const lineStart = box.value.lastIndexOf("\n", start - 1) + 1;
        const before = box.value.slice(lineStart, start);
        if (start !== box.selectionEnd || !/^ +$/.test(before)) return;
        e.preventDefault();
        const indent = before.slice(0, Math.max(0, before.length - 4));
        box.value = box.value.slice(0, lineStart) + indent + "}" + box.value.slice(start);
        box.selectionStart = box.selectionEnd = lineStart + indent.length + 1;
        box.dispatchEvent(new Event("input", { bubbles: true }));
    } else if (e.key === "Tab" && !e.shiftKey) {
        e.preventDefault();
        const start = box.selectionStart, end = box.selectionEnd;
        box.value = box.value.slice(0, start) + "    " + box.value.slice(end);
        box.selectionStart = box.selectionEnd = start + 4;
        box.dispatchEvent(new Event("input", { bubbles: true }));
    }
});
