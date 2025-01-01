function onPositionChange() {
    const positionSelect = document.getElementById("position-select");
    const assignedCarSelect = document.getElementById("assigned-car-select");

    const selectedPosition = positionSelect.value;

    if (selectedPosition === "Driver") {
        assignedCarSelect.disabled = false;
    } else {
        assignedCarSelect.disabled = true;
        assignedCarSelect.value = "";
    }
}