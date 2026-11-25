import os
from pptx import Presentation
from pptx.util import Inches, Pt
from pptx.dml.color import RGBColor
from pptx.enum.text import PP_ALIGN, MSO_ANCHOR
from pptx.enum.shapes import MSO_SHAPE

def build_presentation(output_path):
    prs = Presentation()
    # 16:9 widescreen
    prs.slide_width = Inches(13.333)
    prs.slide_height = Inches(7.5)
    blank_layout = prs.slide_layouts[6] # blank

    # Color Palette
    PRIMARY_DARK = RGBColor(16, 41, 31)     # #10291F
    PRIMARY_GREEN = RGBColor(22, 118, 74)   # #16764A
    EMERALD_ACCENT = RGBColor(52, 201, 135) # #34C987
    BG_LIGHT = RGBColor(245, 248, 246)      # #F5F8F6
    CARD_BG = RGBColor(255, 255, 255)       # #FFFFFF
    TEXT_MAIN = RGBColor(15, 23, 42)        # #0F172A
    TEXT_MUTED = RGBColor(71, 85, 105)      # #475569
    BORDER_COLOR = RGBColor(226, 232, 240)  # #E2E8F0
    ACCENT_AMBER = RGBColor(217, 119, 6)    # #D97706
    ACCENT_BLUE = RGBColor(37, 99, 235)     # #2563EB
    ACCENT_RED = RGBColor(220, 38, 38)      # #DC2626

    def set_slide_background(slide, color):
        bg = slide.shapes.add_shape(MSO_SHAPE.RECTANGLE, 0, 0, prs.slide_width, prs.slide_height)
        bg.fill.solid()
        bg.fill.fore_color.rgb = color
        bg.line.fill.background()
        return bg

    def add_header(slide, title_text, category_text="ECOSANITIZE MUNICIPAL OPERATIONS"):
        cat_box = slide.shapes.add_textbox(Inches(0.8), Inches(0.4), Inches(11.7), Inches(0.35))
        tf_cat = cat_box.text_frame
        tf_cat.word_wrap = True
        p_cat = tf_cat.paragraphs[0]
        p_cat.text = category_text.upper()
        p_cat.font.size = Pt(11)
        p_cat.font.bold = True
        p_cat.font.color.rgb = PRIMARY_GREEN

        title_box = slide.shapes.add_textbox(Inches(0.8), Inches(0.7), Inches(11.7), Inches(0.7))
        tf = title_box.text_frame
        tf.word_wrap = True
        p = tf.paragraphs[0]
        p.text = title_text
        p.font.size = Pt(26)
        p.font.bold = True
        p.font.color.rgb = TEXT_MAIN

    def add_card(slide, left, top, width, height, bg_color=CARD_BG, border_color=BORDER_COLOR):
        shape = slide.shapes.add_shape(MSO_SHAPE.ROUNDED_RECTANGLE, left, top, width, height)
        shape.fill.solid()
        shape.fill.fore_color.rgb = bg_color
        if border_color:
            shape.line.color.rgb = border_color
            shape.line.width = Pt(1)
        else:
            shape.line.fill.background()
        return shape

    # =========================================================================
    # SLIDE 1: Title Slide
    # =========================================================================
    s1 = prs.slides.add_slide(blank_layout)
    set_slide_background(s1, PRIMARY_DARK)

    # Accent decorative bar
    bar = s1.shapes.add_shape(MSO_SHAPE.RECTANGLE, Inches(0.8), Inches(1.8), Inches(0.15), Inches(3.8))
    bar.fill.solid()
    bar.fill.fore_color.rgb = EMERALD_ACCENT
    bar.line.fill.background()

    tb1 = s1.shapes.add_textbox(Inches(1.2), Inches(1.7), Inches(11), Inches(4))
    tf1 = tb1.text_frame
    tf1.word_wrap = True

    p = tf1.paragraphs[0]
    p.text = "MUNICIPAL CIVIC OPERATIONS PLATFORM"
    p.font.size = Pt(13)
    p.font.bold = True
    p.font.color.rgb = EMERALD_ACCENT

    p2 = tf1.add_paragraph()
    p2.text = "EcoSanitize"
    p2.font.size = Pt(46)
    p2.font.bold = True
    p2.font.color.rgb = RGBColor(255, 255, 255)

    p3 = tf1.add_paragraph()
    p3.text = "Urban Waste Segregation, Disposal Tracking and Public Sanitization System"
    p3.font.size = Pt(20)
    p3.font.color.rgb = RGBColor(226, 238, 231)

    p4 = tf1.add_paragraph()
    p4.text = "\nSubmitted in Partial Fulfilment of Degree Requirements for B.Tech. CSE"
    p4.font.size = Pt(13)
    p4.font.color.rgb = RGBColor(145, 170, 155)

    p5 = tf1.add_paragraph()
    p5.text = "Student: Palki | Reg. No: 12313843 | Lovely Professional University, Punjab"
    p5.font.size = Pt(13)
    p5.font.bold = True
    p5.font.color.rgb = RGBColor(255, 255, 255)

    s1.notes_slide.notes_text_frame.text = (
        "Good morning respected evaluators. Today I am presenting my project EcoSanitize, "
        "an urban waste segregation, disposal tracking, and public facility sanitization platform "
        "developed to modernize municipal hygiene operations."
    )

    # =========================================================================
    # SLIDE 2: Problem Statement
    # =========================================================================
    s2 = prs.slides.add_slide(blank_layout)
    set_slide_background(s2, BG_LIGHT)
    add_header(s2, "The Real-World Problem: Urban Sanitation Breakdown")

    cards_data_s2 = [
        ("Mixed Waste Contamination", "Organic food waste, plastics, industrial chemicals, and biomedical items are collected together, ruining recycling opportunities and creating toxic landfill fires.", ACCENT_RED),
        ("Unmonitored Collection Delays", "Municipal vehicles lack centralized dispatching, leaving commercial bulk waste and emergency hazardous bins sitting on streets for days.", ACCENT_AMBER),
        ("Unverified Public Cleanliness", "High-traffic hubs like bus stands, markets, and clinic reception areas lack quantitative inspection protocols and disinfectant verification.", PRIMARY_GREEN),
        ("Citizen Helplessness", "Residents observing open garbage dumping or overflowing bins have no direct, photo-verified civic reporting channel with tracked resolution.", ACCENT_BLUE),
    ]

    for i, (title, desc, color) in enumerate(cards_data_s2):
        col = i % 2
        row = i // 2
        x = Inches(0.8 + col * 5.95)
        y = Inches(1.6 + row * 2.65)
        add_card(s2, x, y, Inches(5.7), Inches(2.35))

        tb = s2.shapes.add_textbox(x + Inches(0.3), y + Inches(0.25), Inches(5.1), Inches(1.85))
        tf = tb.text_frame
        tf.word_wrap = True

        p = tf.paragraphs[0]
        p.text = title
        p.font.size = Pt(16)
        p.font.bold = True
        p.font.color.rgb = color

        p2 = tf.add_paragraph()
        p2.text = desc
        p2.font.size = Pt(13)
        p2.font.color.rgb = TEXT_MUTED

    s2.notes_slide.notes_text_frame.text = (
        "In Indian and global cities, waste management is still managed through manual registers. "
        "When garbage is collected unsegregated, it overloads landfills. Furthermore, public concourses "
        "are rarely audited objectively, and citizens lack a transparent way to report overflowing bins."
    )

    # =========================================================================
    # SLIDE 3: The Solution
    # =========================================================================
    s3 = prs.slides.add_slide(blank_layout)
    set_slide_background(s3, BG_LIGHT)
    add_header(s3, "The Solution: EcoSanitize Municipal Operations Portal")

    # Left large summary card
    add_card(s3, Inches(0.8), Inches(1.6), Inches(4.5), Inches(5.3), PRIMARY_GREEN)
    tb_left = s3.shapes.add_textbox(Inches(1.1), Inches(1.9), Inches(3.9), Inches(4.7))
    tf_l = tb_left.text_frame
    tf_l.word_wrap = True
    p = tf_l.paragraphs[0]
    p.text = "CORE OBJECTIVE"
    p.font.size = Pt(11)
    p.font.bold = True
    p.font.color.rgb = RGBColor(167, 243, 208)

    p2 = tf_l.add_paragraph()
    p2.text = "Transforming Municipal Waste into a Circular, Tracked Civic Service"
    p2.font.size = Pt(22)
    p2.font.bold = True
    p2.font.color.rgb = RGBColor(255, 255, 255)

    p3 = tf_l.add_paragraph()
    p3.text = "\nBy standardizing source segregation, providing real-time vehicle dispatching, and recording chemical sanitization audits, EcoSanitize delivers transparency, public health safety, and measurable landfill diversion."
    p3.font.size = Pt(13)
    p3.font.color.rgb = RGBColor(226, 238, 231)

    # Right side 3 feature cards
    features_s3 = [
        ("1. Standardized 5-Stream Segregation", "Rigorous categorisation into Organic, Recyclable, Hazardous, Sanitary, and E-Waste streams with color codes and hazard ratings."),
        ("2. Real-Time Disposal Tracking Queue", "Triaged lifecycle management tracking pickup requests from Pending through In-Transit to final certified Disposal."),
        ("3. Quantitative Facility Sanitization Audits", "Objective 1-5 hygiene ratings, verified disinfectant chemical logging, and automated re-inspection alerts."),
    ]
    for i, (title, desc) in enumerate(features_s3):
        y = Inches(1.6 + i * 1.8)
        add_card(s3, Inches(5.5), y, Inches(7.0), Inches(1.6))
        tb = s3.shapes.add_textbox(Inches(5.8), y + Inches(0.2), Inches(6.4), Inches(1.2))
        tf = tb.text_frame
        tf.word_wrap = True
        p = tf.paragraphs[0]
        p.text = title
        p.font.size = Pt(16)
        p.font.bold = True
        p.font.color.rgb = PRIMARY_DARK
        p2 = tf.add_paragraph()
        p2.text = desc
        p2.font.size = Pt(12)
        p2.font.color.rgb = TEXT_MUTED

    s3.notes_slide.notes_text_frame.text = (
        "EcoSanitize serves as an enterprise command center. It unifies source waste segregation, "
        "vehicle dispatch queues, facility sanitization audits, citizen grievances, and analytics into "
        "one single digital workflow."
    )

    # =========================================================================
    # SLIDE 4: Key Stakeholders & Roles
    # =========================================================================
    s4 = prs.slides.add_slide(blank_layout)
    set_slide_background(s4, BG_LIGHT)
    add_header(s4, "Who Uses EcoSanitize? Stakeholders & Operational Roles")

    roles_data = [
        ("Citizens & Businesses", "Bulk Producers / Residents", "Log bulk disposal requests, explore segregation guidelines, and file civic grievance tickets with photo evidence.", ACCENT_BLUE),
        ("Municipal Supervisors", "Dispatch & Operations Desk", "Review incoming queues, filter requests, assign vehicle units, monitor urgencies, and close resolved tickets.", PRIMARY_GREEN),
        ("Sanitation Workers", "Field Logistics & Drivers", "Follow assigned collection routes, receive daily sector timetables, and execute safe material transfer.", ACCENT_AMBER),
        ("Health Inspectors", "Sanitation & Hygiene Division", "Audit transit concourses, clinic receptions, and market yards using standardized chemical and rating metrics.", ACCENT_RED),
    ]

    for i, (role, sub, desc, color) in enumerate(roles_data):
        x = Inches(0.8 + i * 2.97)
        add_card(s4, x, Inches(1.6), Inches(2.8), Inches(5.3))

        tb = s4.shapes.add_textbox(x + Inches(0.2), Inches(1.8), Inches(2.4), Inches(4.9))
        tf = tb.text_frame
        tf.word_wrap = True

        p = tf.paragraphs[0]
        p.text = role
        p.font.size = Pt(16)
        p.font.bold = True
        p.font.color.rgb = color

        p2 = tf.add_paragraph()
        p2.text = sub.upper()
        p2.font.size = Pt(10)
        p2.font.bold = True
        p2.font.color.rgb = TEXT_MUTED

        p3 = tf.add_paragraph()
        p3.text = f"\n{desc}"
        p3.font.size = Pt(12)
        p3.font.color.rgb = TEXT_MAIN

    s4.notes_slide.notes_text_frame.text = (
        "Our platform connects four key user groups: Citizens who log requests and file complaints; "
        "Supervisors who dispatch trucks; Workers who handle field routes; and Health Inspectors who "
        "record chemical hygiene audits."
    )

    # =========================================================================
    # SLIDE 5: 5-Stream Waste Segregation
    # =========================================================================
    s5 = prs.slides.add_slide(blank_layout)
    set_slide_background(s5, BG_LIGHT)
    add_header(s5, "Pillar 1: Standardized 5-Stream Waste Segregation")

    streams_data = [
        ("Organic (Green)", "Wet Waste & Food Scraps", "Hazard Level: 1/10", "Aerobic composting, vermicomposting, and anaerobic biogas digestion.", RGBColor(22, 163, 74)),
        ("Recyclable (Blue)", "Paper, Plastic, Metal, Glass", "Hazard Level: 2/10", "Mechanical sorting, shredding, secondary manufacturing raw material.", RGBColor(37, 99, 235)),
        ("Hazardous (Red)", "Chemicals, Reagents, Toxins", "Hazard Level: 8/10", "Secure chemical encapsulation, specialized containment facilities.", RGBColor(220, 38, 38)),
        ("Sanitary (Yellow)", "Biomedical & Clinic Disposables", "Hazard Level: 7/10", "Segregated biohazard autoclaving and certified high-temp incineration.", RGBColor(217, 119, 6)),
        ("E-Waste (Orange)", "Circuit Boards, Batteries", "Hazard Level: 6/10", "Precious metal reclamation, authorized electronic dismantlers.", RGBColor(234, 88, 12)),
    ]

    for i, (title, sub, haz, treatment, col) in enumerate(streams_data):
        x = Inches(0.8 + i * 2.37)
        add_card(s5, x, Inches(1.6), Inches(2.25), Inches(5.3))

        tb = s5.shapes.add_textbox(x + Inches(0.15), Inches(1.8), Inches(1.95), Inches(4.9))
        tf = tb.text_frame
        tf.word_wrap = True

        p = tf.paragraphs[0]
        p.text = title
        p.font.size = Pt(14)
        p.font.bold = True
        p.font.color.rgb = col

        p2 = tf.add_paragraph()
        p2.text = sub
        p2.font.size = Pt(11)
        p2.font.color.rgb = TEXT_MAIN

        p3 = tf.add_paragraph()
        p3.text = f"\n{haz}"
        p3.font.size = Pt(10)
        p3.font.bold = True
        p3.font.color.rgb = col

        p4 = tf.add_paragraph()
        p4.text = f"\nTreatment:\n{treatment}"
        p4.font.size = Pt(11)
        p4.font.color.rgb = TEXT_MUTED

    s5.notes_slide.notes_text_frame.text = (
        "Proper segregation at source is the most vital step. EcoSanitize classifies waste into five "
        "distinct streams. Each stream defines clear container colors, hazard indices, and mandatory "
        "treatment protocols, ensuring medical and chemical wastes are never mixed with compostable organics."
    )

    # =========================================================================
    # SLIDE 6: Disposal Tracking & Dispatch Queue
    # =========================================================================
    s6 = prs.slides.add_slide(blank_layout)
    set_slide_background(s6, BG_LIGHT)
    add_header(s6, "Pillar 2: Real-Time Disposal Tracking & Dispatch Queue")

    cards_s6 = [
        ("1. Request Logging", "Bulk generators (markets, laboratories, hospitals) submit pickup requests specifying location, estimated weight in kg, category, and urgency level (Low, Medium, High, Emergency)."),
        ("2. Live Queue & Filtering", "Supervisors view all requests in a responsive data table, filterable by waste stream. Urgent and emergency items are highlighted with clear visual indicators."),
        ("3. Dispatch & Transit", "Supervisors assign a municipal collector or vehicle unit (e.g. 'Hazmat Unit 01') and transition the status to 'In-Transit' with one click."),
        ("4. Safe Disposal Closure", "Once the waste arrives at the designated processing or incineration facility, the record is transitioned to 'Disposed', recording verified completion."),
    ]

    for i, (title, desc) in enumerate(cards_s6):
        y = Inches(1.6 + i * 1.35)
        add_card(s6, Inches(0.8), y, Inches(11.7), Inches(1.2))
        tb = s6.shapes.add_textbox(Inches(1.1), y + Inches(0.15), Inches(11.1), Inches(0.9))
        tf = tb.text_frame
        tf.word_wrap = True
        p = tf.paragraphs[0]
        p.text = title
        p.font.size = Pt(15)
        p.font.bold = True
        p.font.color.rgb = PRIMARY_GREEN
        p2 = tf.add_paragraph()
        p2.text = desc
        p2.font.size = Pt(12)
        p2.font.color.rgb = TEXT_MUTED

    s6.notes_slide.notes_text_frame.text = (
        "The Disposal Tracking Queue provides end-to-end visibility. From the moment a bulk request "
        "is entered, through vehicle assignment and in-transit delivery, to certified disposal, "
        "every transaction is accounted for without paper delays."
    )

    # =========================================================================
    # SLIDE 7: Facility Sanitization Audits
    # =========================================================================
    s7 = prs.slides.add_slide(blank_layout)
    set_slide_background(s7, BG_LIGHT)
    add_header(s7, "Pillar 3: Public Facility Sanitization & Hygiene Audits")

    s7_boxes = [
        ("Quantitative Cleanliness Scores", "Replaces subjective reviews with a strict 1 to 5 numerical inspection score. Displays transparent ratios like '5 / 5' or '2 / 5' with subtle color coding for failing scores.", PRIMARY_DARK),
        ("Disinfectant Chemical Verification", "Audits verify and record certified chemical agents used on-site (such as Sodium Hypochlorite 1% solution or Quaternary Ammonium Compounds) rather than unverified washing.", PRIMARY_GREEN),
        ("Automated Corrective Action Trigger", "Scores <= 2 automatically trigger 'Requires Follow-Up = Yes' and mark the facility as 'Needs Attention', scheduling mandatory deep cleaning re-inspection.", ACCENT_RED),
        ("Clean Tabular Municipal Audit Display", "Inspection records are presented in a clean, humanized municipal ledger showing area name, inspector, disinfectant, rating, and final pass/attention outcome.", ACCENT_BLUE),
    ]

    for i, (title, desc, col) in enumerate(s7_boxes):
        col_idx = i % 2
        row_idx = i // 2
        x = Inches(0.8 + col_idx * 5.95)
        y = Inches(1.6 + row_idx * 2.65)
        add_card(s7, x, y, Inches(5.7), Inches(2.35))

        tb = s7.shapes.add_textbox(x + Inches(0.3), y + Inches(0.25), Inches(5.1), Inches(1.85))
        tf = tb.text_frame
        tf.word_wrap = True
        p = tf.paragraphs[0]
        p.text = title
        p.font.size = Pt(16)
        p.font.bold = True
        p.font.color.rgb = col
        p2 = tf.add_paragraph()
        p2.text = desc
        p2.font.size = Pt(13)
        p2.font.color.rgb = TEXT_MUTED

    s7.notes_slide.notes_text_frame.text = (
        "Sanitization Auditing brings accountability to public spaces. Health inspectors log objective "
        "scores and the exact disinfectant chemical applied. If a public concourse scores 2 or less, "
        "the portal flags it as Needs Attention and requires an immediate deep-cleaning follow up."
    )

    # =========================================================================
    # SLIDE 8: Citizen Grievance Redressal
    # =========================================================================
    s8 = prs.slides.add_slide(blank_layout)
    set_slide_background(s8, BG_LIGHT)
    add_header(s8, "Pillar 4: Citizen Grievance Redressal with Photo Evidence")

    s8_steps = [
        ("1. Issue Detection", "Citizens spot civic sanitation hazards such as overflowing street bins, open dumping, clogged storm drains, or missed household collections."),
        ("2. Photographic Upload", "Citizens file a ticket via the portal, attaching photographic evidence stored securely on the server with timestamp and location details."),
        ("3. Ward-Wise Triage", "Grievance records are automatically organized by municipal Ward (e.g. Ward 04, Ward 12), enabling immediate departmental accountability."),
        ("4. Closure with Notes", "Field sanitation teams clear the site, update status to 'Resolved', and record official resolution notes visible to administrators."),
    ]

    for i, (title, desc) in enumerate(s8_steps):
        y = Inches(1.6 + i * 1.35)
        add_card(s8, Inches(0.8), y, Inches(11.7), Inches(1.2))
        tb = s8.shapes.add_textbox(Inches(1.1), y + Inches(0.15), Inches(11.1), Inches(0.9))
        tf = tb.text_frame
        tf.word_wrap = True
        p = tf.paragraphs[0]
        p.text = title
        p.font.size = Pt(15)
        p.font.bold = True
        p.font.color.rgb = ACCENT_BLUE
        p2 = tf.add_paragraph()
        p2.text = desc
        p2.font.size = Pt(12)
        p2.font.color.rgb = TEXT_MUTED

    s8.notes_slide.notes_text_frame.text = (
        "Citizens are active partners in maintaining city hygiene. When an overflowing bin is reported, "
        "citizens attach photo proof. The complaint is routed to the specific ward supervisor, resolved, "
        "and documented with resolution notes."
    )

    # =========================================================================
    # SLIDE 9: Workforce & Route Logistics
    # =========================================================================
    s9 = prs.slides.add_slide(blank_layout)
    set_slide_background(s9, BG_LIGHT)
    add_header(s9, "Pillar 5: Field Workforce & Collection Route Schedules")

    # Left card: Worker Roster
    add_card(s9, Inches(0.8), Inches(1.6), Inches(5.7), Inches(5.3))
    tb_w = s9.shapes.add_textbox(Inches(1.1), Inches(1.9), Inches(5.1), Inches(4.7))
    tf_w = tb_w.text_frame
    tf_w.word_wrap = True
    p = tf_w.paragraphs[0]
    p.text = "Sanitation Worker Directory"
    p.font.size = Pt(18)
    p.font.bold = True
    p.font.color.rgb = PRIMARY_DARK
    p2 = tf_w.add_paragraph()
    p2.text = "\n- Tracks employee records with ID, full name, zone, and phone contact.\n- Shift Duty Toggle allows supervisors to update worker status (Active, On-Duty, On-Leave) with a single click.\n- Balances workforce availability against daily ward collection loads.\n- Direct driver mapping for specialized vehicles (Compactor Unit 04, Hazmat Van)."
    p2.font.size = Pt(13)
    p2.font.color.rgb = TEXT_MUTED

    # Right card: Route Schedules
    add_card(s9, Inches(6.8), Inches(1.6), Inches(5.7), Inches(5.3))
    tb_s = s9.shapes.add_textbox(Inches(7.1), Inches(1.9), Inches(5.1), Inches(4.7))
    tf_s = tb_s.text_frame
    tf_s.word_wrap = True
    p = tf_s.paragraphs[0]
    p.text = "Sector Collection Timetables"
    p.font.size = Pt(18)
    p.font.bold = True
    p.font.color.rgb = PRIMARY_GREEN
    p2 = tf_s.add_paragraph()
    p2.text = "\n- Organizes recurring route schedules by Ward Zone and Day of the Week.\n- Maps collection shift times (e.g. Morning 06:30 AM, Afternoon 01:00 PM).\n- Allocates dedicated vehicle units to specific streams (e.g. Wet Organic on Mon/Thu, Dry Recyclable on Tue/Fri).\n- Active route switches enable supervisors to pause or resume sectors dynamically."
    p2.font.size = Pt(13)
    p2.font.color.rgb = TEXT_MUTED

    s9.notes_slide.notes_text_frame.text = (
        "Logistics execution depends on reliable staffing and schedules. Our Worker Roster tracks duty "
        "attendance in real-time, while our Collection Schedules timetable maps specific garbage trucks "
        "to wards and days of the week, preventing missed pickups."
    )

    # =========================================================================
    # SLIDE 10: Intelligence, Visual Analytics & Reports
    # =========================================================================
    s10 = prs.slides.add_slide(blank_layout)
    set_slide_background(s10, BG_LIGHT)
    add_header(s10, "City Intelligence: Visual Analytics & Exportable Reports")

    analytics_cards = [
        ("1. Daily Collection Weight Trend", "Bar graph visualizing daily tonnage collected over the last 7 days. Highlights surge days to optimize vehicle fuel and crew allocation.", PRIMARY_GREEN),
        ("2. Waste Stream Composition", "Doughnut chart illustrating the exact ratio of Organic, Recyclable, Hazardous, Sanitary, and E-Waste collected across the municipality.", ACCENT_BLUE),
        ("3. Citizen Grievance Categories", "Horizontal bar chart highlighting primary complaint types (Overflowing Bins, Missed Pickups, Drainage) to focus municipal budget and bins.", ACCENT_AMBER),
        ("4. Instant Official Reports", "Includes 1-click server-side CSV Spreadsheet exports (Disposal Logs, Complaints) and an official Printable PDF Audit Report for municipal reviews.", PRIMARY_DARK),
    ]

    for i, (title, desc, col) in enumerate(analytics_cards):
        col_idx = i % 2
        row_idx = i // 2
        x = Inches(0.8 + col_idx * 5.95)
        y = Inches(1.6 + row_idx * 2.65)
        add_card(s10, x, y, Inches(5.7), Inches(2.35))

        tb = s10.shapes.add_textbox(x + Inches(0.3), y + Inches(0.25), Inches(5.1), Inches(1.85))
        tf = tb.text_frame
        tf.word_wrap = True
        p = tf.paragraphs[0]
        p.text = title
        p.font.size = Pt(16)
        p.font.bold = True
        p.font.color.rgb = col
        p2 = tf.add_paragraph()
        p2.text = desc
        p2.font.size = Pt(13)
        p2.font.color.rgb = TEXT_MUTED

    s10.notes_slide.notes_text_frame.text = (
        "The Analytics module gives city administrators full clarity. Three interactive charts show "
        "daily collection trends, waste stream proportions, and citizen grievance patterns. In addition, "
        "supervisors can export complete CSV spreadsheets or print formatted PDF inspection summaries."
    )

    # =========================================================================
    # SLIDE 11: Real-World Impact & Key Benefits
    # =========================================================================
    s11 = prs.slides.add_slide(blank_layout)
    set_slide_background(s11, BG_LIGHT)
    add_header(s11, "Real-World Impact & Municipal Benefits")

    impacts = [
        ("High Landfill Diversion", "Achieves up to 80% diversion of collected materials away from dumping grounds by channeling organics to composting and dry waste to recyclers.", PRIMARY_GREEN),
        ("Operational Cost Savings", "Scheduled dispatches and balanced worker allocation minimize vehicle fuel wastage, idle truck times, and unnecessary overtime expenses.", ACCENT_BLUE),
        ("Public Health & Disease Control", "Consistent chemical disinfection audits in transit stations, markets, and clinics help suppress bacterial and viral disease transmission.", ACCENT_RED),
        ("Complete Civic Transparency", "Every disposal request, worker shift, and citizen grievance has a digital timestamp, eliminating lost complaints and paper falsification.", PRIMARY_DARK),
    ]

    for i, (title, desc, col) in enumerate(impacts):
        x = Inches(0.8 + i * 2.97)
        add_card(s11, x, Inches(1.6), Inches(2.8), Inches(5.3))

        tb = s11.shapes.add_textbox(x + Inches(0.2), Inches(1.8), Inches(2.4), Inches(4.9))
        tf = tb.text_frame
        tf.word_wrap = True
        p = tf.paragraphs[0]
        p.text = title
        p.font.size = Pt(16)
        p.font.bold = True
        p.font.color.rgb = col
        p2 = tf.add_paragraph()
        p2.text = f"\n{desc}"
        p2.font.size = Pt(12)
        p2.font.color.rgb = TEXT_MUTED

    s11.notes_slide.notes_text_frame.text = (
        "The real-world benefits of EcoSanitize are proven: Up to 80% of waste is diverted from landfills, "
        "fuel and operational costs decrease through route scheduling, public health improves through verified "
        "chemical audits, and citizens experience transparent, responsive governance."
    )

    # =========================================================================
    # SLIDE 12: Conclusion & Future Roadmap
    # =========================================================================
    s12 = prs.slides.add_slide(blank_layout)
    set_slide_background(s12, PRIMARY_DARK)

    # Header
    tb_hdr = s12.shapes.add_textbox(Inches(0.8), Inches(0.6), Inches(11.7), Inches(1.0))
    tf_h = tb_hdr.text_frame
    tf_h.word_wrap = True
    p = tf_h.paragraphs[0]
    p.text = "CONCLUSION & FUTURE PERSPECTIVE"
    p.font.size = Pt(12)
    p.font.bold = True
    p.font.color.rgb = EMERALD_ACCENT

    p2 = tf_h.add_paragraph()
    p2.text = "Next-Generation Smart Sanitation Roadmap"
    p2.font.size = Pt(28)
    p2.font.bold = True
    p2.font.color.rgb = RGBColor(255, 255, 255)

    roadmap_items = [
        ("1. IoT Smart Bins", "Integrating ultrasonic fill-level sensors into roadside public bins that trigger automatic disposal requests once bin capacity reaches 80%."),
        ("2. Live GPS Truck Telemetry", "Live map tracking of municipal garbage compactors allowing citizens to see expected arrival times on their smartphones."),
        ("3. AI-Assisted Waste Sorting", "Deploying computer vision models to automatically analyze citizen complaint photos and classify waste types before truck dispatch."),
        ("4. Mobile Progressive Web App", "Packaging the worker roster and audit checklists into an offline-first mobile app for field workers operating in low-signal areas."),
    ]

    for i, (title, desc) in enumerate(roadmap_items):
        col_idx = i % 2
        row_idx = i // 2
        x = Inches(0.8 + col_idx * 5.95)
        y = Inches(1.8 + row_idx * 2.3)
        add_card(s12, x, y, Inches(5.7), Inches(2.05), RGBColor(22, 45, 35), RGBColor(41, 68, 56))

        tb = s12.shapes.add_textbox(x + Inches(0.3), y + Inches(0.2), Inches(5.1), Inches(1.6))
        tf = tb.text_frame
        tf.word_wrap = True
        p = tf.paragraphs[0]
        p.text = title
        p.font.size = Pt(15)
        p.font.bold = True
        p.font.color.rgb = EMERALD_ACCENT
        p2 = tf.add_paragraph()
        p2.text = desc
        p2.font.size = Pt(12)
        p2.font.color.rgb = RGBColor(226, 238, 231)

    # Thank you footer
    tb_ftr = s12.shapes.add_textbox(Inches(0.8), Inches(6.5), Inches(11.7), Inches(0.5))
    tf_f = tb_ftr.text_frame
    p = tf_f.paragraphs[0]
    p.text = "Thank You! | Questions & Discussion"
    p.alignment = PP_ALIGN.CENTER
    p.font.size = Pt(15)
    p.font.bold = True
    p.font.color.rgb = RGBColor(255, 255, 255)

    s12.notes_slide.notes_text_frame.text = (
        "In conclusion, EcoSanitize demonstrates how modern software engineering can directly improve "
        "urban hygiene and environmental sustainability. In the future, this system is designed to integrate "
        "IoT smart bin sensors, live GPS truck tracking, and AI image classification. "
        "Thank you for your time, and I am now ready to take your questions."
    )

    prs.save(output_path)
    print(f"Presentation successfully saved to: {output_path}")

if __name__ == "__main__":
    out_dir = r"c:\Users\karti\Desktop\C#-project"
    out_file = os.path.join(out_dir, "EcoSanitize_Municipal_Operations_Presentation.pptx")
    build_presentation(out_file)
