window.gridUtils = {
    initResizing: function (gridId) {
        console.log("initResizing called for " + gridId);
    },
    onColumnResized: function (dotNetHelper, groupId) {
        // Blazor Bootstrap Grid doesn't have a direct "onColumnResized" event exposed easily in JS
        // for standard HTML table columns, but if we use a custom table or have a way to intercept
        // the grid's behavior, we'd do it here.
        // For now, we'll provide a placeholder that can be called from Blazor or observed via MutationObserver.
    },
    saveColumnWidths: function (groupId, widths) {
        localStorage.setItem('grid_widths_' + groupId, JSON.stringify(widths));
    }
};
