export type SkillItem = {
  id: number;
  name: string;
  category: string;
  isActive: boolean;
  updatedAtUtc: string;
};

export type ExtractionLogItem = {
  id: number;
  createdAtUtc: string;
  skillCount: number;
  warningCount: number;
  summary: string;
};
