window.gridUtils = {
    initResizing: function (gridId) {
        // Delay slightly to let Blazor finish rendering
        setTimeout(() => {
            const grid = document.getElementById(gridId);
            if (!grid) return;

            const cols = grid.querySelectorAll('th');
            cols.forEach(col => {
                // Remove existing resizer if any (for re-initialization)
                const existingResizer = col.querySelector('.resizer');
                if (existingResizer) {
                    existingResizer.remove();
                }

                // Create resizer element
                const resizer = document.createElement('div');
                resizer.classList.add('resizer');
                col.appendChild(resizer);

                function getColumnName() {
                    const input = col.querySelector('input');
                    if (input) return input.value.trim();
                    return col.innerText.trim();
                }

                resizer.addEventListener('mousedown', function initDrag(e) {
                    e.preventDefault();
                    e.stopPropagation(); // prevent sorting/editing
                    const startX = e.pageX;
                    const startWidth = col.offsetWidth;
                    
                    function doDrag(e) {
                        const newWidth = Math.max(50, startWidth + (e.pageX - startX));
                        col.style.width = newWidth + 'px';
                        col.style.minWidth = newWidth + 'px';
                        col.style.maxWidth = newWidth + 'px';
                    }

                    function stopDrag() {
                        document.removeEventListener('mousemove', doDrag);
                        document.removeEventListener('mouseup', stopDrag);
                        
                        const colName = getColumnName();
                        if (colName) {
                            localStorage.setItem(`col-width-${gridId}-${colName}`, col.style.width);
                        }
                    }

                    document.addEventListener('mousemove', doDrag);
                    document.addEventListener('mouseup', stopDrag);
                });

                // Restore widths
                const colName = getColumnName();
                if (colName) {
                    const savedWidth = localStorage.getItem(`col-width-${gridId}-${colName}`);
                    if (savedWidth) {
                        col.style.width = savedWidth;
                        col.style.minWidth = savedWidth;
                        col.style.maxWidth = savedWidth;
                    }
                }
            });
        }, 100);
    },
    copyToClipboard: async function (text) {
        try {
            await navigator.clipboard.writeText(text);
            return true;
        } catch (err) {
            console.error('Failed to copy text: ', err);
            return false;
        }
    }
};
