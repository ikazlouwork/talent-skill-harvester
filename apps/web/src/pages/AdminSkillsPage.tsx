import { FormEvent, useCallback, useEffect, useMemo, useState } from "react";
import { Card } from "../components/Card";
import { StatusBlock } from "../components/StatusBlock";
import { webConfig } from "../config";
import type { SkillItem } from "../types/admin";

export function AdminSkillsPage() {
  const [skills, setSkills] = useState<SkillItem[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [errorText, setErrorText] = useState<string | null>(null);
  const [successText, setSuccessText] = useState<string | null>(null);

  const [createName, setCreateName] = useState("");
  const [createCategory, setCreateCategory] = useState("");
  const [createIsActive, setCreateIsActive] = useState(true);

  const [editingSkillId, setEditingSkillId] = useState<number | null>(null);
  const [editName, setEditName] = useState("");
  const [editCategory, setEditCategory] = useState("");
  const [editIsActive, setEditIsActive] = useState(true);

  const loadSkills = useCallback(async () => {
    setIsLoading(true);

    try {
      const response = await fetch(`${webConfig.apiBaseUrl}/api/skills`);

      if (!response.ok) {
        const payload = (await response.json().catch(() => null)) as { error?: string } | null;
        setErrorText(payload?.error ?? `Failed to load skills: ${response.status}`);
        return;
      }

      const payload = (await response.json()) as SkillItem[];
      setSkills(payload);
      setErrorText(null);
    } catch {
      setErrorText("Failed to reach API endpoint while loading skills.");
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    void loadSkills();
  }, [loadSkills]);

  const canSubmitCreate = useMemo(() => {
    return !isSubmitting;
  }, [isSubmitting]);

  const canSubmitEdit = useMemo(() => {
    return !isSubmitting && editingSkillId !== null;
  }, [editingSkillId, isSubmitting]);

  async function handleCreateSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    if (!canSubmitCreate) {
      return;
    }

    if (createName.trim().length === 0 || createCategory.trim().length === 0) {
      setErrorText("Name and category are required to create a skill.");
      setSuccessText(null);
      return;
    }

    setIsSubmitting(true);
    setErrorText(null);
    setSuccessText(null);

    try {
      const response = await fetch(`${webConfig.apiBaseUrl}/api/skills`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify({
          name: createName.trim(),
          category: createCategory.trim(),
          isActive: createIsActive
        })
      });

      if (!response.ok) {
        const payload = (await response.json().catch(() => null)) as { error?: string } | null;
        setErrorText(payload?.error ?? `Create request failed with status ${response.status}`);
        return;
      }

      setCreateName("");
      setCreateCategory("");
      setCreateIsActive(true);
      setSuccessText("Skill created successfully.");
      await loadSkills();
    } catch {
      setErrorText("Failed to reach API endpoint while creating a skill.");
    } finally {
      setIsSubmitting(false);
    }
  }

  function beginEdit(skill: SkillItem) {
    setEditingSkillId(skill.id);
    setEditName(skill.name);
    setEditCategory(skill.category);
    setEditIsActive(skill.isActive);
    setErrorText(null);
    setSuccessText(null);
  }

  function cancelEdit() {
    setEditingSkillId(null);
    setEditName("");
    setEditCategory("");
    setEditIsActive(true);
  }

  async function handleEditSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    if (!canSubmitEdit || editingSkillId === null) {
      return;
    }

    if (editName.trim().length === 0 || editCategory.trim().length === 0) {
      setErrorText("Name and category are required to update a skill.");
      setSuccessText(null);
      return;
    }

    setIsSubmitting(true);
    setErrorText(null);
    setSuccessText(null);

    try {
      const response = await fetch(`${webConfig.apiBaseUrl}/api/skills/${editingSkillId}`, {
        method: "PATCH",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify({
          name: editName.trim(),
          category: editCategory.trim(),
          isActive: editIsActive
        })
      });

      if (!response.ok) {
        const payload = (await response.json().catch(() => null)) as { error?: string } | null;
        setErrorText(payload?.error ?? `Update request failed with status ${response.status}`);
        return;
      }

      setSuccessText("Skill updated successfully.");
      cancelEdit();
      await loadSkills();
    } catch {
      setErrorText("Failed to reach API endpoint while updating a skill.");
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <div className="page-grid">
      <Card title="Admin Skills">
        <p>Manage skills catalog with list, create, and edit workflows.</p>
        <p className="muted-text">Changes are persisted in SQLite through API endpoints.</p>
      </Card>

      {(isLoading || errorText || successText) && (
        <Card title="Skills status">
          <StatusBlock
            loadingText="Loading skills..."
            errorText={errorText}
            successText={!errorText ? successText : null}
          />
        </Card>
      )}

      <Card title="Current skills">
        {!isLoading && skills.length === 0 && <p>No skills found. Create your first skill below.</p>}

        {skills.length > 0 && (
          <ul className="plain-list">
            {skills.map((skill) => (
              <li key={skill.id} className="admin-list-item">
                <div>
                  <strong>{skill.name}</strong> ({skill.category}) — {skill.isActive ? "active" : "inactive"}
                  <div className="muted-text">Updated: {new Date(skill.updatedAtUtc).toLocaleString()}</div>
                </div>
                <button className="secondary-button" type="button" onClick={() => beginEdit(skill)}>
                  Edit
                </button>
              </li>
            ))}
          </ul>
        )}
      </Card>

      <Card title="Create skill">
        <form className="extract-form" onSubmit={handleCreateSubmit}>
          <label className="field-label" htmlFor="createSkillName">
            Name
          </label>
          <input
            className="field-input"
            id="createSkillName"
            name="createSkillName"
            value={createName}
            onChange={(event) => setCreateName(event.target.value)}
            placeholder="e.g. Azure Cosmos DB"
          />

          <label className="field-label" htmlFor="createSkillCategory">
            Category
          </label>
          <input
            className="field-input"
            id="createSkillCategory"
            name="createSkillCategory"
            value={createCategory}
            onChange={(event) => setCreateCategory(event.target.value)}
            placeholder="e.g. Database"
          />

          <label className="field-checkbox" htmlFor="createSkillIsActive">
            <input
              id="createSkillIsActive"
              name="createSkillIsActive"
              type="checkbox"
              checked={createIsActive}
              onChange={(event) => setCreateIsActive(event.target.checked)}
            />
            Active
          </label>

          <button className="primary-button" type="submit" disabled={!canSubmitCreate}>
            {isSubmitting ? "Saving..." : "Create skill"}
          </button>
        </form>
      </Card>

      <Card title="Edit skill">
        {editingSkillId === null && <p>Select any skill from the list to edit it.</p>}

        {editingSkillId !== null && (
          <form className="extract-form" onSubmit={handleEditSubmit}>
            <label className="field-label" htmlFor="editSkillName">
              Name
            </label>
            <input
              className="field-input"
              id="editSkillName"
              name="editSkillName"
              value={editName}
              onChange={(event) => setEditName(event.target.value)}
            />

            <label className="field-label" htmlFor="editSkillCategory">
              Category
            </label>
            <input
              className="field-input"
              id="editSkillCategory"
              name="editSkillCategory"
              value={editCategory}
              onChange={(event) => setEditCategory(event.target.value)}
            />

            <label className="field-checkbox" htmlFor="editSkillIsActive">
              <input
                id="editSkillIsActive"
                name="editSkillIsActive"
                type="checkbox"
                checked={editIsActive}
                onChange={(event) => setEditIsActive(event.target.checked)}
              />
              Active
            </label>

            <div className="admin-actions">
              <button className="primary-button" type="submit" disabled={!canSubmitEdit}>
                {isSubmitting ? "Saving..." : "Update skill"}
              </button>
              <button className="secondary-button" type="button" onClick={cancelEdit}>
                Cancel
              </button>
            </div>
          </form>
        )}
      </Card>
    </div>
  );
}