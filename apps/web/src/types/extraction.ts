export type ExtractRequest = {
  cvText: string;
  ifuText: string;
};

export type ExtractedSkill = {
  name: string;
  category: string;
  evidence: string;
  confidence: number;
};

export type ExtractResponse = {
  summary: string;
  skills: ExtractedSkill[];
  warnings: string[];
};

export type ResultsLocationState = {
  result: ExtractResponse;
};
