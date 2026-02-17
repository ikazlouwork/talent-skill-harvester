import { FormEvent, useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Card } from "../components/Card";
import { StatusBlock } from "../components/StatusBlock";
import { webConfig } from "../config";
import type { ExtractResponse } from "../types/extraction";

export function ExtractPage() {
  const navigate = useNavigate();
  const [cvText, setCvText] = useState("");
  const [ifuText, setIfuText] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [errorText, setErrorText] = useState<string | null>(null);

  const canSubmit = useMemo(() => {
    return !isSubmitting;
  }, [isSubmitting]);

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    if (!canSubmit) {
      return;
    }

    if (cvText.trim().length === 0 || ifuText.trim().length === 0) {
      setErrorText("Please fill in both CV text and IFU text before extraction.");
      return;
    }

    setErrorText(null);
    setIsSubmitting(true);

    try {
      const response = await fetch(`${webConfig.apiBaseUrl}/api/extract`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify({
          cvText,
          ifuText
        })
      });

      if (!response.ok) {
        const payload = (await response.json().catch(() => null)) as { error?: string } | null;
        const errorMessage = payload?.error ?? `Extraction failed with status ${response.status}`;
        setErrorText(errorMessage);
        return;
      }

      const payload = (await response.json()) as ExtractResponse;

      navigate("/results", {
        state: {
          result: payload
        }
      });
    } catch {
      setErrorText("Extraction error: failed to reach API endpoint");
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <div className="page-grid">
      <Card title="Extract skills from CV + IFU">
        <form className="extract-form" onSubmit={handleSubmit}>
          <label className="field-label" htmlFor="cvText">
            CV text
          </label>
          <textarea
            className="field-textarea"
            id="cvText"
            name="cvText"
            value={cvText}
            onChange={(event) => setCvText(event.target.value)}
            rows={8}
            placeholder="Paste candidate CV content"
          />

          <label className="field-label" htmlFor="ifuText">
            IFU text
          </label>
          <textarea
            className="field-textarea"
            id="ifuText"
            name="ifuText"
            value={ifuText}
            onChange={(event) => setIfuText(event.target.value)}
            rows={6}
            placeholder="Paste IFU / role requirements"
          />

          <button className="primary-button" type="submit" disabled={!canSubmit}>
            {isSubmitting ? "Extracting..." : "Extract skills"}
          </button>
        </form>
      </Card>

      {(isSubmitting || errorText) && (
        <Card title="Extraction status">
          <StatusBlock
            loadingText="Extraction in progress..."
            errorText={errorText}
            successText={null}
          />
        </Card>
      )}

      {!isSubmitting && !errorText && (
        <Card title="How this works">
          <p>1. Paste CV and IFU text.</p>
          <p>2. Submit to the mock extractor API.</p>
          <p>3. Review structured skills on the results page.</p>
        </Card>
      )}
    </div>
  );
}