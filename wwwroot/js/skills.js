document.addEventListener("DOMContentLoaded", () => {
    const input = document.getElementById("skill-input");
    const tagsContainer = document.getElementById("skill-tags");
    const hiddenContainer = document.getElementById("skills-container");

    // Track current skill index (account for pre-existing skills)
    let skillIndex = hiddenContainer.querySelectorAll("input[name$='.SkillName']").length;

    function getExistingSkills() {
        return [...tagsContainer.querySelectorAll(".skill-tag")]
            .map(tag => tag.dataset.skill?.toLowerCase());
    }

    function addSkill(skillName) {
        skillName = skillName.trim().replace(/,/g, "");

        if (!skillName) return;

        // Prevent duplicates
        if (getExistingSkills().includes(skillName.toLowerCase())) {
            input.value = "";
            return;
        }

        // --- Add visual tag ---
        const tag = document.createElement("span");
        tag.className = "skill-tag";
        tag.dataset.skill = skillName;
        tag.innerHTML = `
            ${skillName}
            <button type="button" class="remove-tag" data-skill="${skillName}">×</button>
        `;

        // Remove tag + hidden inputs on click
        tag.querySelector(".remove-tag").addEventListener("click", () => {
            removeSkill(skillName, tag);
        });

        tagsContainer.appendChild(tag);

        // --- Add hidden inputs for model binding ---
        const userId = document.querySelector("input[name='User.Id']").value;

        const idInput = document.createElement("input");
        idInput.type = "hidden";
        idInput.name = `Skills[${skillIndex}].Id`;
        idInput.value = "0"; // new skill

        const userIdInput = document.createElement("input");
        userIdInput.type = "hidden";
        userIdInput.name = `Skills[${skillIndex}].UserId`;
        userIdInput.value = userId;

        const nameInput = document.createElement("input");
        nameInput.type = "hidden";
        nameInput.name = `Skills[${skillIndex}].SkillName`;
        nameInput.value = skillName;
        nameInput.dataset.skillKey = skillName; // used for removal

        hiddenContainer.appendChild(idInput);
        hiddenContainer.appendChild(userIdInput);
        hiddenContainer.appendChild(nameInput);

        skillIndex++;
        input.value = "";
    }

    function removeSkill(skillName, tagEl) {
        // Remove visual tag
        tagEl.remove();

        // Remove hidden inputs for this skill and re-index remaining
        const allNameInputs = hiddenContainer.querySelectorAll("input[name$='.SkillName']");
        allNameInputs.forEach(nameInput => {
            if (nameInput.dataset.skillKey === skillName) {
                // Find and remove the Id and UserId inputs at the same index
                const indexMatch = nameInput.name.match(/\[(\d+)\]/);
                if (indexMatch) {
                    const idx = indexMatch[1];
                    hiddenContainer.querySelector(`input[name='Skills[${idx}].Id']`)?.remove();
                    hiddenContainer.querySelector(`input[name='Skills[${idx}].UserId']`)?.remove();
                    nameInput.remove();
                }
            }
        });

        // Re-index all remaining hidden inputs
        reindexSkills();
    }

    function reindexSkills() {
        const nameInputs = hiddenContainer.querySelectorAll("input[name$='.SkillName']");
        nameInputs.forEach((nameInput, i) => {
            const oldIndex = nameInput.name.match(/\[(\d+)\]/)[1];
            hiddenContainer.querySelector(`input[name='Skills[${oldIndex}].Id']`).name = `Skills[${i}].Id`;
            hiddenContainer.querySelector(`input[name='Skills[${oldIndex}].UserId']`).name = `Skills[${i}].UserId`;
            nameInput.name = `Skills[${i}].SkillName`;
        });
        skillIndex = nameInputs.length;
    }

    // Trigger on comma key
    input.addEventListener("keydown", (e) => {
        if (e.key === ",") {
            e.preventDefault();
            addSkill(input.value);
        }
        // Also allow Enter
        if (e.key === "Enter") {
            e.preventDefault();
            addSkill(input.value);
        }
    });

    // Handle paste (e.g. pasting "React, Vue, Angular")
    input.addEventListener("paste", (e) => {
        e.preventDefault();
        const pasted = e.clipboardData.getData("text");
        pasted.split(",").forEach(skill => addSkill(skill));
    });

    // Click wrapper to focus input
    document.querySelector(".skills-input-wrapper").addEventListener("click", () => {
        input.focus();
    });
});