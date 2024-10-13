let currentDate = new Date();

function getMonday(d) {
    d = new Date(d);
    var day = d.getDay(),
        diff = d.getDate() - day + (day == 0 ? -6 : 1); // adjust when day is sunday
    return new Date(d.setDate(diff));
}

function formatDate(date) {
    return `${date.getDate()}/${date.getMonth() + 1}`;
}

function updateCalendar() {
    let monday = getMonday(currentDate);

    for (let i = 0; i < 7; i++) {
        let date = new Date(monday);
        date.setDate(monday.getDate() + i);
        document.getElementById(`day${i}`).textContent = `${['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'][date.getDay()]} ${formatDate(date)}`;
    }

    let scheduleBody = document.getElementById('scheduleBody');
    scheduleBody.innerHTML = ''; // Clear existing schedule

    for (let slot = 1; slot <= 4; slot++) {
        let row = document.createElement('tr');
        let slotCell = document.createElement('td');
        slotCell.textContent = `Slot ${slot}`;
        row.appendChild(slotCell);

        for (let day = 0; day < 7; day++) {
            let cell = document.createElement('td');
            let timeSlot = document.createElement('div');
            timeSlot.className = 'time-slot';

            let timeSpan = document.createElement('span');
            timeSpan.textContent = getSlotTime(slot);
            timeSlot.appendChild(timeSpan);

            let statusBadge = document.createElement('span');
            statusBadge.className = 'badge bg-warning text-dark';
            statusBadge.textContent = 'Unavailable';
            timeSlot.appendChild(statusBadge);

            let button = document.createElement('button');
            button.className = 'btn btn-success btn-sm';
            button.textContent = 'Mark as Available';
            button.onclick = toggleAvailability;
            timeSlot.appendChild(button);

            cell.appendChild(timeSlot);
            row.appendChild(cell);
        }

        scheduleBody.appendChild(row);
    }
}

function getSlotTime(slot) {
    switch (slot) {
        case 1: return '07:00 - 09:15';
        case 2: return '09:30 - 11:45';
        case 3: return '12:30 - 02:45';
        case 4: return '03:00 - 05:15';
    }
}

function toggleAvailability(event) {
    const button = event.target;
    const timeSlot = button.parentElement;
    const statusBadge = timeSlot.querySelector('.badge');

    if (button.textContent === 'Mark as Available') {
        button.textContent = 'Mark as Unavailable';
        button.className = 'btn btn-warning btn-sm';
        statusBadge.textContent = 'Available';
        statusBadge.className = 'badge bg-success';
    } else {
        button.textContent = 'Mark as Available';
        button.className = 'btn btn-success btn-sm';
        statusBadge.textContent = 'Unavailable';
        statusBadge.className = 'badge bg-warning text-dark';
    }
}

document.getElementById('prevWeek').addEventListener('click', () => {
    currentDate.setDate(currentDate.getDate() - 7);
    updateCalendar();
});

document.getElementById('nextWeek').addEventListener('click', () => {
    currentDate.setDate(currentDate.getDate() + 7);
    updateCalendar();
});

// Initial calendar update
updateCalendar();
