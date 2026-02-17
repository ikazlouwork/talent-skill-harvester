type StatusBlockProps = {
  loadingText: string;
  errorText: string | null;
  successText: string | null;
};

export function StatusBlock({ loadingText, errorText, successText }: StatusBlockProps) {
  if (errorText) {
    return (
      <div className="status-block status-block-error" role="status" aria-live="polite">
        <p className="status-error">{errorText}</p>
      </div>
    );
  }

  if (successText) {
    return (
      <div className="status-block status-block-success" role="status" aria-live="polite">
        <p className="status-success">{successText}</p>
      </div>
    );
  }

  return (
    <div className="status-block status-block-loading" role="status" aria-live="polite">
      <p className="status-loading">{loadingText}</p>
    </div>
  );
}