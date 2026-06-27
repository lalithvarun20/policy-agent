# Data Classification Standard
## L&W Corporate

**Standard ID:** DCS-2025-v2
**Effective:** January 15, 2026
**Owner:** Office of the CISO
**Audience:** All L&W personnel handling L&W or customer data

---

## §1. Purpose

This standard defines L&W's data classification framework, the controls required for each classification level, and the responsibilities of data owners, custodians, and users. It exists to ensure L&W applies appropriate protection commensurate with the sensitivity and regulatory exposure of the data it handles.

## §2. Scope

§2.1. This standard applies to all data created, received, stored, or transmitted by L&W, regardless of format (electronic, paper, verbal) or location (on-premises, cloud, mobile, third-party).

§2.2. This standard is read in conjunction with the Acceptable Use Policy (AUP-2025) and the Vendor Management and Third-Party Risk Policy (VM-2025).

## §3. Classification levels

L&W classifies data into four levels in increasing order of sensitivity.

### §3.1. Public

§3.1.1. Data that L&W has authorized for unrestricted release. Includes published marketing materials, public regulatory filings, press releases, and L&W's public website content.

§3.1.2. Public data may be shared without restriction inside or outside L&W.

§3.1.3. Public data requires no specific handling controls beyond standard hygiene.

### §3.2. Internal

§3.2.1. Data intended for internal L&W use that does not require additional protection beyond access controls. Includes internal communications not falling into a higher classification, organizational announcements, and most operational data.

§3.2.2. Internal data may be shared within L&W on L&W-authorized systems. Sharing outside L&W requires the approval of the data owner.

§3.2.3. Internal data requires authentication-controlled access. Encryption at rest and in transit using L&W standard practices.

### §3.3. Confidential

§3.3.1. Data whose unauthorized disclosure would cause material harm to L&W's business, customer relationships, or competitive position. Includes financial information not yet public, strategic plans, M&A activity, internal investigation records, executive communications on sensitive matters, and customer business information shared with L&W in confidence.

§3.3.2. Confidential data requires data-owner approval before being shared outside the originating team within L&W. Sharing outside L&W requires the additional approval of the Office of the CISO.

§3.3.3. Confidential data requires authentication-controlled access with role-based authorization. Encryption at rest and in transit using L&W standard practices. Access logging is mandatory. Data Loss Prevention (DLP) controls apply.

### §3.4. Restricted

§3.4.1. Data whose unauthorized disclosure would cause severe harm to L&W, its customers, its employees, or other affected parties. Includes regulated customer data subject to gaming-industry rules, personally identifiable information (PII), payment card information (PCI scope), authentication credentials, source code for revenue-generating systems, and trade secrets.

§3.4.2. Restricted data may be shared only as specifically authorized by the data owner and the Office of the CISO, and only through approved channels.

§3.4.3. Restricted data requires authentication-controlled access with role-based authorization, plus an explicit access grant for each accessing party. Encryption at rest and in transit using L&W standard practices, plus additional encryption layers as defined per data type. Access logging is mandatory with active monitoring. Data Loss Prevention (DLP) controls apply with strict enforcement.

## §4. Customer data classification

§4.1. Customer data is classified per the contractual commitments L&W has made to the customer and per applicable regulation.

§4.2. In the absence of a specific classification, customer data defaults to Confidential.

§4.3. Customer data that includes PII, PCI, or regulated gaming data is classified as Restricted regardless of contractual silence.

## §5. AI tool authorization by classification

§5.1. The L&W AI Tooling Standard documents the data classifications authorized for each L&W-approved AI tool. The standard is maintained by the Office of the CISO and updated as the tool landscape evolves.

§5.2. As of the effective date of this standard, the authorized classifications are:

§5.2.1. **Public and Internal data.** Microsoft Copilot for M365, GitHub Copilot for Business, and the internal L&W Agent Platform.

§5.2.2. **Confidential data.** The internal L&W Agent Platform only. Microsoft Copilot for M365 (Restricted Tier configuration only, available to specific personnel).

§5.2.3. **Restricted data.** No AI tools are currently authorized for use with Restricted data without per-engagement approval from the Office of the CISO.

§5.3. Use of any AI tool with data above its authorized classification level is a violation of this standard and the AUP.

## §6. Data ownership

§6.1. Every data set has a designated data owner, typically the functional leader of the team that originated the data.

§6.2. The data owner is responsible for classification decisions, access grants, and review of access lists for the data they own.

§6.3. Data custodians (typically IT or platform engineering personnel) are responsible for implementing the technical controls required by the classification.

## §7. Data retention

§7.1. L&W's Records Retention Schedule defines retention periods by data category. The Records Retention Schedule is maintained by Legal in coordination with Finance and Compliance.

§7.2. Data classified as Restricted may not be retained beyond the retention period without affirmative re-authorization by the data owner.

## §8. Incident response

§8.1. Suspected data exposure events involving Confidential or Restricted data must be reported to the Office of the CISO within 4 hours of discovery, with status updates every 24 hours until resolved.

§8.2. Suspected data exposure events involving Public or Internal data are reported through the standard AUP incident reporting process (AUP §9).

## §9. Revision history

| Version | Date | Summary |
|---|---|---|
| v1 | 2022-11-01 | Initial issuance |
| v2 | 2026-01-15 | Added AI tool authorization section (§5); customer data clarification (§4) |
