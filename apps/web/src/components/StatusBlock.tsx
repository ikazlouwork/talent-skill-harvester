type StatusBlockProps = {
  loadingText: string;
  errorText: string | null;
  successText: string | null;
};

export function StatusBlock({ loadingText, errorText, successText }: StatusBlockProps) {
  if (errorText) {
    return <p>{errorText}</p>;
  }

  if (successText) {
    return <p>{successText}</p>;
  }

  return <p>{loadingText}</p>;
}