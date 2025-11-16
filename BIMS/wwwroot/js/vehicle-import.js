// AEGIS IBMS - Vehicle Import from Excel

let selectedFile = null;

// Show import modal
function showImportModal() {
    document.getElementById('vehicleImportModal').style.display = 'flex';
    // Reset file selection
    selectedFile = null;
    document.getElementById('vehicleImportFile').value = '';
    document.getElementById('selectedFileName').textContent = '';
    document.getElementById('btnStartImport').disabled = true;
    document.getElementById('importProgress').style.display = 'none';
}

// Close import modal
function closeImportModal() {
    document.getElementById('vehicleImportModal').style.display = 'none';
}

// Handle file selection
function handleFileSelect(input) {
    const file = input.files[0];
    if (file) {
        // Validate file type
        const fileName = file.name.toLowerCase();
        if (!fileName.endsWith('.xlsx') && !fileName.endsWith('.xls')) {
            const currentLang = localStorage.getItem('aegis-language') || 'en';
            alert(currentLang === 'ar' ? 'نوع الملف غير صحيح. يجب أن يكون Excel' : 'Invalid file type. Must be Excel file');
            input.value = '';
            return;
        }

        // Validate file size (5MB)
        if (file.size > 5 * 1024 * 1024) {
            const currentLang = localStorage.getItem('aegis-language') || 'en';
            alert(currentLang === 'ar' ? 'حجم الملف كبير جداً (الحد الأقصى 5MB)' : 'File size too large (max 5MB)');
            input.value = '';
            return;
        }

        selectedFile = file;
        document.getElementById('selectedFileName').textContent = file.name;
        document.getElementById('btnStartImport').disabled = false;
    }
}

// Start import process
async function startImport() {
    if (!selectedFile) {
        const currentLang = localStorage.getItem('aegis-language') || 'en';
        alert(currentLang === 'ar' ? 'الرجاء اختيار ملف' : 'Please select a file');
        return;
    }

    // Get customer ID from URL or hidden field
    const customerId = getCustomerId();
    if (!customerId) {
        const currentLang = localStorage.getItem('aegis-language') || 'en';
        alert(currentLang === 'ar' ? 'خطأ: معرف العميل غير موجود' : 'Error: Customer ID not found');
        return;
    }

    // Show progress
    document.getElementById('importProgress').style.display = 'block';
    document.getElementById('btnStartImport').disabled = true;
    
    const progressFill = document.querySelector('.progress-fill');
    progressFill.style.width = '50%';

    // Prepare form data
    const formData = new FormData();
    formData.append('customerId', customerId);
    formData.append('excelFile', selectedFile);

    // Get anti-forgery token
    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

    try {
        const response = await fetch('/Customer/ImportVehiclesFromExcel', {
            method: 'POST',
            headers: {
                'RequestVerificationToken': token
            },
            body: formData
        });

        progressFill.style.width = '100%';

        const result = await response.json();

        // Hide import modal
        closeImportModal();

        // Show results modal
        showResultsModal(result);

    } catch (error) {
        console.error('Import error:', error);
        const currentLang = localStorage.getItem('aegis-language') || 'en';
        alert(currentLang === 'ar' ? `خطأ: ${error.message}` : `Error: ${error.message}`);
        document.getElementById('importProgress').style.display = 'none';
        document.getElementById('btnStartImport').disabled = false;
    }
}

// Show results modal
function showResultsModal(result) {
    if (!result.success) {
        alert(result.message);
        return;
    }

    // Update statistics
    document.getElementById('successCount').textContent = result.successCount || 0;
    document.getElementById('errorCount').textContent = result.errorCount || 0;
    document.getElementById('totalCount').textContent = result.totalRows || 0;

    // Show/hide errors section
    const errorsListDiv = document.getElementById('errorsList');
    const errorsTableBody = document.getElementById('errorsTableBody');
    
    if (result.errors && result.errors.length > 0) {
        errorsListDiv.style.display = 'block';
        
        // Clear existing rows
        errorsTableBody.innerHTML = '';
        
        // Add error rows
        result.errors.forEach(error => {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${error.rowNumber}</td>
                <td>${error.make || '-'}</td>
                <td>${error.model || '-'}</td>
                <td style="color: #dc3545;">${error.errorMessage}</td>
            `;
            errorsTableBody.appendChild(row);
        });
    } else {
        errorsListDiv.style.display = 'none';
    }

    // Show modal
    document.getElementById('importResultsModal').style.display = 'flex';
}

// Close results modal
function closeResultsModal() {
    document.getElementById('importResultsModal').style.display = 'none';
}

// Reload page to see imported vehicles
function reloadPage() {
    window.location.reload();
}

// Get customer ID from URL
function getCustomerId() {
    // Try to get from URL path (e.g., /Customer/EditCustomer/5)
    const path = window.location.pathname;
    const match = path.match(/EditCustomer\/(\d+)/);
    
    if (match && match[1]) {
        return match[1];
    }
    
    // Try to get from hidden input
    const hiddenInput = document.querySelector('input[name="Id"]');
    if (hiddenInput) {
        return hiddenInput.value;
    }
    
    return null;
}

// Close modals when clicking outside
window.addEventListener('click', function(event) {
    const importModal = document.getElementById('vehicleImportModal');
    const resultsModal = document.getElementById('importResultsModal');
    
    if (event.target === importModal) {
        closeImportModal();
    }
    if (event.target === resultsModal) {
        closeResultsModal();
    }
});

// Handle Escape key to close modals
document.addEventListener('keydown', function(event) {
    if (event.key === 'Escape') {
        closeImportModal();
        closeResultsModal();
    }
});