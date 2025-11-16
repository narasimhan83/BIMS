// AEGIS IBMS - Customer Form Dynamic Functionality

document.addEventListener('DOMContentLoaded', function() {
    initializeCustomerForm();
});

function initializeCustomerForm() {
    // Initialize conditional group dropdown
    const customerTypeDropdown = document.getElementById('CustomerTypeId');
    if (customerTypeDropdown) {
        customerTypeDropdown.addEventListener('change', toggleGroupDropdown);
        // Trigger on page load in case of edit mode
        toggleGroupDropdown();
    }
}

// ============================================
// CONDITIONAL GROUP DROPDOWN
// ============================================

function toggleGroupDropdown() {
    const customerTypeDropdown = document.getElementById('CustomerTypeId');
    const groupSection = document.getElementById('groupSection');
    const groupDropdown = document.getElementById('CustomerGroupId');
    
    if (!customerTypeDropdown || !groupSection) return;
    
    const selectedText = customerTypeDropdown.options[customerTypeDropdown.selectedIndex]?.text || '';
    
    // Show group dropdown if "Group" or "مجموعة" is selected
    if (selectedText.toLowerCase().includes('group') || selectedText.includes('مجموعة')) {
        groupSection.style.display = 'block';
        groupSection.classList.add('slide-in');
    } else {
        groupSection.style.display = 'none';
        if (groupDropdown) {
            groupDropdown.value = '';
        }
    }
}

// ============================================
// DYNAMIC DOCUMENT MANAGEMENT
// ============================================

let documentIndex = 0;

function addDocument() {
    const container = document.getElementById('documentsContainer');
    if (!container) return;
    
    // Clear helper text on first document add
    const helperText = container.querySelector('.helper-text');
    if (helperText) {
        helperText.remove();
    }
    
    const currentLang = localStorage.getItem('aegis-language') || 'en';
    const removeText = currentLang === 'ar' ? 'إزالة' : 'Remove';
    const docTypeText = currentLang === 'ar' ? 'نوع المستند' : 'Document Type';
    const fileText = currentLang === 'ar' ? 'الملف' : 'File';
    
    const currentIndex = documentIndex;
    const row = document.createElement('div');
    row.className = 'document-row';
    row.setAttribute('data-doc-index', currentIndex);
    row.innerHTML = `
        <div class="document-fields">
            <div class="form-group">
                <label>${docTypeText}</label>
                <select name="DocumentType_${currentIndex}" class="form-control doc-type-select" data-index="${currentIndex}" required>
                    <option value="">-- ${currentLang === 'ar' ? 'اختر...' : 'Select...'} --</option>
                    ${getDocumentTypesOptions()}
                </select>
            </div>
            <div class="form-group">
                <label>${fileText}</label>
                <input type="file" name="DocumentFile_${currentIndex}" class="form-control doc-file-input" data-index="${currentIndex}" accept=".pdf,.jpg,.jpeg,.png,.docx" required />
            </div>
            <div class="form-group">
                <button type="button" class="btn-remove-doc" onclick="removeDocument(this)">
                    <span>🗑️</span> ${removeText}
                </button>
            </div>
        </div>
    `;
    
    container.appendChild(row);
    documentIndex++;
}

function removeDocument(button) {
    const row = button.closest('.document-row');
    if (row) {
        row.classList.add('fade-out');
        setTimeout(() => row.remove(), 300);
    }
}

function getDocumentTypesOptions() {
    // This will be populated from ViewBag in the actual view
    // Placeholder for demonstration
    return '';
}

// ============================================
// DYNAMIC VEHICLE MANAGEMENT
// ============================================

let vehicleIndex = 0;

function addVehicle() {
    const container = document.getElementById('vehiclesContainer');
    if (!container) return;
    
    // Clear helper text on first vehicle add
    const helperText = container.querySelector('.helper-text');
    if (helperText) {
        helperText.remove();
    }
    
    const currentLang = localStorage.getItem('aegis-language') || 'en';
    const removeText = currentLang === 'ar' ? 'إزالة' : 'Remove';
    
    const row = document.createElement('div');
    row.className = 'vehicle-row';
    row.innerHTML = `
        <div class="vehicle-fields">
            <div class="vehicle-row-grid">
                <div class="form-group">
                    <label>${currentLang === 'ar' ? 'الصانع' : 'Make'}</label>
                    <select name="Vehicles[${vehicleIndex}].VehicleMakeId" class="form-control vehicle-make" 
                            onchange="loadModels(this, ${vehicleIndex})">
                        <option value="">-- ${currentLang === 'ar' ? 'اختر...' : 'Select...'} --</option>
                        ${getVehicleMakesOptions()}
                    </select>
                </div>
                <div class="form-group">
                    <label>${currentLang === 'ar' ? 'الموديل' : 'Model'}</label>
                    <select name="Vehicles[${vehicleIndex}].VehicleModelId" class="form-control vehicle-model-${vehicleIndex}">
                        <option value="">-- ${currentLang === 'ar' ? 'اختر...' : 'Select...'} --</option>
                    </select>
                </div>
                <div class="form-group">
                    <label>${currentLang === 'ar' ? 'السنة' : 'Year'}</label>
                    <select name="Vehicles[${vehicleIndex}].VehicleYearId" class="form-control">
                        <option value="">-- ${currentLang === 'ar' ? 'اختر...' : 'Select...'} --</option>
                        ${getVehicleYearsOptions()}
                    </select>
                </div>
                <div class="form-group">
                    <label>${currentLang === 'ar' ? 'السعة' : 'Capacity'}</label>
                    <select name="Vehicles[${vehicleIndex}].EngineCapacityId" class="form-control">
                        <option value="">-- ${currentLang === 'ar' ? 'اختر...' : 'Select...'} --</option>
                        ${getEngineCapacitiesOptions()}
                    </select>
                </div>
            </div>
            <div class="vehicle-row-grid">
                <div class="form-group">
                    <label>${currentLang === 'ar' ? 'رقم التسجيل' : 'Registration#'}</label>
                    <input type="text" name="Vehicles[${vehicleIndex}].RegistrationNumber" class="form-control" />
                </div>
                <div class="form-group">
                    <label>${currentLang === 'ar' ? 'رقم الشاسيه' : 'Chassis#'}</label>
                    <input type="text" name="Vehicles[${vehicleIndex}].ChassisNumber" class="form-control" />
                </div>
                <div class="form-group">
                    <button type="button" class="btn-remove-vehicle" onclick="removeVehicle(this)">
                        <span>🗑️</span> ${removeText}
                    </button>
                </div>
            </div>
        </div>
    `;
    
    container.appendChild(row);
    vehicleIndex++;
}

function removeVehicle(button) {
    const row = button.closest('.vehicle-row');
    if (row) {
        row.classList.add('fade-out');
        setTimeout(() => row.remove(), 300);
    }
}

// Cascading dropdown: Load models based on selected make
function loadModels(makeDropdown, index) {
    const makeId = makeDropdown.value;
    const modelDropdown = document.querySelector(`.vehicle-model-${index}`);
    
    if (!modelDropdown || !makeId) {
        if (modelDropdown) {
            modelDropdown.innerHTML = '<option value="">-- Select Model --</option>';
        }
        return;
    }
    
    // Load models via AJAX
    fetch(`/Customer/GetModelsByMake?makeId=${makeId}`)
        .then(response => response.json())
        .then(models => {
            const currentLang = localStorage.getItem('aegis-language') || 'en';
            let options = `<option value="">-- ${currentLang === 'ar' ? 'اختر الموديل' : 'Select Model'} --</option>`;
            
            models.forEach(model => {
                // ASP.NET Core serializes to camelCase, so use lowercase property names
                const displayName = model.modelName || '';
                options += `<option value="${model.id}">${displayName}</option>`;
            });
            
            modelDropdown.innerHTML = options;
        })
        .catch(error => console.error('Error loading models:', error));
}

// Generate dropdown options from server data
function getDocumentTypesOptions() {
    if (!window.documentTypesData || window.documentTypesData.length === 0) {
        console.warn('Document types data not loaded', window.documentTypesData);
        return '';
    }
    console.log('Document types data:', window.documentTypesData);
    return window.documentTypesData.map(item => {
        // Handle both camelCase and PascalCase
        const id = item.id || item.Id;
        const displayName = item.displayName || item.DisplayName;
        return `<option value="${id}">${displayName}</option>`;
    }).join('');
}

function getVehicleMakesOptions() {
    if (!window.vehicleMakesData || window.vehicleMakesData.length === 0) {
        console.warn('Vehicle makes data not loaded', window.vehicleMakesData);
        return '';
    }
    console.log('Vehicle makes data:', window.vehicleMakesData);
    return window.vehicleMakesData.map(item => {
        // Handle both camelCase and PascalCase
        const id = item.id || item.Id;
        const displayName = item.displayName || item.DisplayName;
        return `<option value="${id}">${displayName}</option>`;
    }).join('');
}

function getVehicleYearsOptions() {
    if (!window.vehicleYearsData || window.vehicleYearsData.length === 0) {
        console.warn('Vehicle years data not loaded', window.vehicleYearsData);
        return '';
    }
    console.log('Vehicle years data:', window.vehicleYearsData);
    return window.vehicleYearsData.map(item => {
        // Handle both camelCase and PascalCase
        const id = item.id || item.Id;
        const displayName = item.displayName || item.DisplayName;
        return `<option value="${id}">${displayName}</option>`;
    }).join('');
}

function getEngineCapacitiesOptions() {
    if (!window.engineCapacitiesData || window.engineCapacitiesData.length === 0) {
        console.warn('Engine capacities data not loaded', window.engineCapacitiesData);
        return '';
    }
    console.log('Engine capacities data:', window.engineCapacitiesData);
    return window.engineCapacitiesData.map(item => {
        // Handle both camelCase and PascalCase
        const id = item.id || item.Id;
        const displayName = item.displayName || item.DisplayName;
        return `<option value="${id}">${displayName}</option>`;
    }).join('');
}

// ============================================
// FORM VALIDATION
// ============================================

function validateCustomerForm() {
    const customerName = document.getElementById('CustomerName')?.value;
    const customerType = document.getElementById('CustomerTypeId')?.value;
    
    if (!customerName || !customerType) {
        alert('Please fill in required fields');
        return false;
    }
    
    return true;
}

// Animation classes
const style = document.createElement('style');
style.textContent = `
    .slide-in {
        animation: slideIn 0.3s ease-out;
    }
    @keyframes slideIn {
        from {
            opacity: 0;
            transform: translateY(-10px);
        }
        to {
            opacity: 1;
            transform: translateY(0);
        }
    }
    .fade-out {
        animation: fadeOut 0.3s ease-out;
    }
    @keyframes fadeOut {
        from {
            opacity: 1;
        }
        to {
            opacity: 0;
        }
    }
`;
document.head.appendChild(style);