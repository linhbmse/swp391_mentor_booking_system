// SwpMentorBooking.Web/wwwroot/js/manageUser.js

document.addEventListener('DOMContentLoaded', function () {
    const studentTable = document.getElementById('studentTable');
    const mentorTable = document.getElementById('mentorTable');
    const searchInput = document.getElementById('searchInput');

    function filterTable(table, searchTerm) {
        const rows = table.getElementsByTagName('tr');
        for (let i = 1; i < rows.length; i++) {
            const row = rows[i];
            const cells = row.getElementsByTagName('td');
            let found = false;
            for (let j = 0; j < cells.length; j++) {
                const cellText = cells[j].textContent || cells[j].innerText;
                if (cellText.toLowerCase().indexOf(searchTerm) > -1) {
                    found = true;
                    break;
                }
            }
            row.style.display = found ? '' : 'none';
        }
    }

    searchInput.addEventListener('keyup', function () {
        const searchTerm = this.value.toLowerCase();
        filterTable(studentTable, searchTerm);
        filterTable(mentorTable, searchTerm);
    });

    const userTypeSwitch = document.getElementById('userTypeSwitch');
    const switchLabel = document.getElementById('switchLabel');

    userTypeSwitch.addEventListener('change', function () {
        if (this.checked) {
            document.getElementById('mentors-tab').click();
            switchLabel.textContent = 'Mentors';
        } else {
            document.getElementById('students-tab').click();
            switchLabel.textContent = 'Students';
        }
    });
});