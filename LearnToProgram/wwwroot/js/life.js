// Drawing for the Game of Life boards. The C# page owns the cells and sends them here to draw:
// one byte per cell, row by row (1 = alive, 0 = dead, 2 = not reached yet, drawn faded).
window.lifeBoard = {
    // Draws the cells on the canvas. The canvas keeps its shape when CSS makes it narrower.
    draw(canvas, rows, cols, cells) {
        if (!canvas) return;
        const size = Math.max(4, Math.min(14, Math.floor(900 / cols)));
        const ratio = window.devicePixelRatio || 1;
        const width = cols * size, height = rows * size;
        if (canvas.width !== width * ratio || canvas.height !== height * ratio) {
            canvas.width = width * ratio;
            canvas.height = height * ratio;
            canvas.style.aspectRatio = cols + " / " + rows;
        }
        const ctx = canvas.getContext("2d");
        ctx.setTransform(ratio, 0, 0, ratio, 0, 0);
        const style = getComputedStyle(canvas);
        const alive = style.getPropertyValue("--life-alive").trim() || "#3b5bdb";
        const dead = style.getPropertyValue("--life-dead").trim() || "#ffffff";
        const later = style.getPropertyValue("--life-later").trim() || "#f3f4f7";
        const line = style.getPropertyValue("--life-line").trim() || "#e3e6eb";
        ctx.fillStyle = line;
        ctx.fillRect(0, 0, width, height);
        const gap = size >= 7 ? 1 : 0;
        for (let r = 0; r < rows; r++) {
            for (let c = 0; c < cols; c++) {
                const v = cells[r * cols + c];
                ctx.fillStyle = v === 1 ? alive : v === 2 ? later : dead;
                ctx.fillRect(c * size, r * size, size - gap, size - gap);
            }
        }
    },

    // Click or drag on the canvas to draw cells. The first cell decides whether the drag turns cells on
    // or off: C#'s Toggle(row, col) answers the new value, and Set(row, col, value) paints the rest.
    attach(canvas, rows, cols, dotnet) {
        if (!canvas || canvas.lifeAttached) return;
        canvas.lifeAttached = true;
        let painting = null, last = "";
        const cellAt = e => {
            const box = canvas.getBoundingClientRect();
            const col = Math.floor((e.clientX - box.left) / box.width * cols);
            const row = Math.floor((e.clientY - box.top) / box.height * rows);
            return row >= 0 && row < rows && col >= 0 && col < cols ? [row, col] : null;
        };
        canvas.addEventListener("pointerdown", async e => {
            const cell = cellAt(e);
            if (!cell) return;
            e.preventDefault();
            canvas.setPointerCapture(e.pointerId);
            last = cell.join(",");
            painting = "wait";
            painting = await dotnet.invokeMethodAsync("Toggle", cell[0], cell[1]);
        });
        canvas.addEventListener("pointermove", e => {
            if (typeof painting !== "boolean") return;
            const cell = cellAt(e);
            if (!cell || cell.join(",") === last) return;
            last = cell.join(",");
            dotnet.invokeMethodAsync("Set", cell[0], cell[1], painting);
        });
        const stop = () => { painting = null; last = ""; };
        canvas.addEventListener("pointerup", stop);
        canvas.addEventListener("pointercancel", stop);
    },
};
